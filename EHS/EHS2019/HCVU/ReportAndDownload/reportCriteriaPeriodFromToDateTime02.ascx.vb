Imports Common.ComFunction.ParameterFunction

Partial Public Class reportCriteriaPeriodFromToDateTime02
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim udtFormatter As New Common.Format.Formatter()

        ' Set Default Value & Style
        Me.calExFromDate.Format = udtFormatter.EnterDateFormat
        Me.calExToDate.Format = udtFormatter.EnterDateFormat

        Me.txtPeriodFrom.Text = ""
        Me.txtFromTime.Text = ""

        Me.txtPeriodTo.Text = ""
        Me.txtToTime.Text = ""

        Me.imgPeriodFromError.Visible = False
        Me.imgPeriodToError.Visible = False

    End Sub

    ' For Store Procedure Parameter
    Public Function GetCriteriaInput() As Common.ComFunction.ParameterFunction.StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamCollection As New StoreProcParamCollection()

        Dim udtFormatter As New Common.Format.Formatter()
        Dim blnNullFrom As Boolean = False
        Dim blnNullTo As Boolean = False
        Dim dtmFrom As DateTime
        Dim dtmTo As DateTime

        If IsDate(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E")) Then
            dtmFrom = udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E")
            dtmFrom = dtmFrom.AddHours(Convert.ToInt32(Me.txtFromTime.Text.Substring(0, 2)))
            dtmFrom = dtmFrom.AddMinutes(Convert.ToInt32(Me.txtFromTime.Text.Substring(3, 2)))
        Else
            blnNullFrom = True
        End If

        If IsDate(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E")) Then
            dtmTo = udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E")
            dtmTo = dtmTo.AddHours(Convert.ToInt32(Me.txtToTime.Text.Substring(0, 2)))
            dtmTo = dtmTo.AddMinutes(Convert.ToInt32(Me.txtToTime.Text.Substring(3, 2)))
        Else
            blnNullTo = True
        End If

        '@param_value01		nvarchar(255),  -- @start_date'
        '@param_value02		nvarchar(255),  -- @start_time
        '@param_value03		nvarchar(255),  -- @end_date
        '@param_value04		nvarchar(255)  -- @end_time

        If blnNullFrom Then
            udtStoreProcParamCollection.AddParam("@param_value01", System.Data.SqlDbType.VarChar, 255, String.Empty)
            udtStoreProcParamCollection.AddParam("@param_value02", System.Data.SqlDbType.VarChar, 255, String.Empty)
        Else
            udtStoreProcParamCollection.AddParam("@param_value01", System.Data.SqlDbType.VarChar, 255, dtmFrom.ToString("yyyy-MMM-dd"))
            udtStoreProcParamCollection.AddParam("@param_value02", System.Data.SqlDbType.VarChar, 255, dtmFrom.ToString("HH:mm"))
        End If

        If blnNullTo Then
            udtStoreProcParamCollection.AddParam("@param_value03", System.Data.SqlDbType.VarChar, 255, String.Empty)
            udtStoreProcParamCollection.AddParam("@param_value04", System.Data.SqlDbType.VarChar, 255, String.Empty)
        Else
            udtStoreProcParamCollection.AddParam("@param_value03", System.Data.SqlDbType.VarChar, 255, dtmTo.ToString("yyyy-MMM-dd"))
            udtStoreProcParamCollection.AddParam("@param_value04", System.Data.SqlDbType.VarChar, 255, dtmTo.ToString("HH:mm"))
        End If

        Return udtStoreProcParamCollection
    End Function

    Public Function GetParameterList() As Common.ComFunction.ParameterFunction.ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterCollection As New ParameterCollection()
        udtParameterCollection.AddParam("Transaction Time From", Me.txtPeriodFrom.Text + " " + Me.txtFromTime.Text.Trim())
        udtParameterCollection.AddParam("Transaction Time To", Me.txtPeriodTo.Text + " " + Me.txtToTime.Text.Trim())
        Return udtParameterCollection
    End Function

    Public Function GetParameterString() As String Implements IReportCriteriaUC.GetParameterString
        Dim strReturn As String = String.Empty
        strReturn = "Transaction Time From:" + Me.txtPeriodFrom.Text + " " + Me.txtFromTime.Text.Trim() + ",Transaction Time To:" + Me.txtPeriodTo.Text + " " + Me.txtToTime.Text.Trim()
        Return strReturn
    End Function

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As System.Collections.Generic.List(Of Common.ComObject.SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        'Me.imgPeriodFromError.Visible = False
        'Me.imgPeriodToError.Visible = False

        'Dim udtFormatter As New Common.Format.Formatter()
        'Me.txtPeriodFrom.Text = udtFormatter.formatInputDate(Me.txtPeriodFrom.Text)
        'Me.txtPeriodTo.Text = udtFormatter.formatInputDate(Me.txtPeriodTo.Text)

        'Dim lstError As List(Of Common.ComObject.SystemMessage) = New List(Of Common.ComObject.SystemMessage)

        '' Init
        'Dim blnInputFromDateTime As Boolean = True
        'Dim blnInputToDateTime As Boolean = True
        'Dim blnValidFromDateTime As Boolean = True
        'Dim blnValidToDateTime As Boolean = True


        '' validation: Input From
        'If Me.txtFromTime.Text = "" AndAlso Me.txtPeriodFrom.Text = "" Then
        '    blnInputFromDateTime = False
        '    'lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00014"))
        '    'Me.imgPeriodFromError.Visible = True
        'Else
        '    blnInputFromDateTime = True
        'End If

        '' validation: Input To
        'If Me.txtToTime.Text = "" AndAlso Me.txtPeriodTo.Text = "" Then
        '    blnInputToDateTime = False
        '    lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00015"))
        '    Me.imgPeriodToError.Visible = True
        'Else
        '    blnInputToDateTime = True
        'End If

        'Dim intDummy As Integer

        '' validation: Input From format
        'If blnInputFromDateTime Then
        '    If Me.txtPeriodFrom.Text <> "" AndAlso Me.txtFromTime.Text <> "" AndAlso Me.txtFromTime.Text.Trim().Length >= 5 Then
        '        Dim blnValidFromDate As Boolean = IsDate(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
        '        Dim blnValidFromHour As Boolean = Integer.TryParse(Me.txtFromTime.Text.Substring(0, 2).Trim(), intDummy)
        '        Dim blnValidFromMin As Boolean = Integer.TryParse(Me.txtFromTime.Text.Substring(3, 2).Trim(), intDummy)

        '        If blnValidFromDate Then
        '            Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
        '            If dtmInputFromDate.Year < 1900 Or dtmInputFromDate > Date.MaxValue Then
        '                blnValidFromDate = False
        '            End If
        '        End If

        '        ' Hour
        '        If blnValidFromHour Then
        '            Dim intHour As Integer = Convert.ToInt32(Me.txtFromTime.Text.Substring(0, 2).Trim())
        '            If intHour < 0 OrElse intHour >= 24 Then
        '                blnValidFromHour = False
        '            End If
        '        End If
        '        ' Min
        '        If blnValidFromMin Then
        '            Dim intMin As Integer = Convert.ToInt32(Me.txtFromTime.Text.Substring(3, 2).Trim())
        '            If intMin < 0 OrElse intMin >= 60 Then
        '                blnValidFromHour = False
        '            End If
        '        End If

        '        If Not (blnValidFromDate AndAlso blnValidFromHour AndAlso blnValidFromMin) Then
        '            blnValidFromDateTime = False
        '        End If
        '    Else
        '        blnValidFromDateTime = False
        '    End If

        '    If Not blnValidFromDateTime Then
        '        lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00016"))
        '        Me.imgPeriodFromError.Visible = True
        '    End If
        'End If

        '' validation: Input To format
        'If blnInputToDateTime Then
        '    If Me.txtToTime.Text.Trim() <> "" AndAlso Me.txtToTime.Text.Trim() <> "" AndAlso Me.txtToTime.Text.Trim().Length >= 5 Then
        '        Dim blnValidToDate As Boolean = IsDate(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
        '        Dim blnValidToHour As Boolean = Integer.TryParse(Me.txtToTime.Text.Substring(0, 2).Trim(), intDummy)
        '        Dim blnValidToMin As Boolean = Integer.TryParse(Me.txtToTime.Text.Substring(3, 2).Trim(), intDummy)


        '        If blnValidToDate Then
        '            Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
        '            If dtmInputToDate.Year < 1900 Or dtmInputToDate > Date.MaxValue Then
        '                blnValidToDate = False
        '            End If
        '        End If

        '        ' Hour
        '        If blnValidToHour Then
        '            Dim intHour As Integer = Convert.ToInt32(Me.txtToTime.Text.Substring(0, 2).Trim())
        '            If intHour < 0 OrElse intHour >= 24 Then
        '                blnValidToHour = False
        '            End If
        '        End If

        '        ' Min
        '        If blnValidToMin Then
        '            Dim intMin As Integer = Convert.ToInt32(Me.txtToTime.Text.Substring(3, 2).Trim())
        '            If intMin < 0 OrElse intMin >= 60 Then
        '                blnValidToMin = False
        '            End If
        '        End If

        '        If Not (blnValidToDate AndAlso blnValidToHour AndAlso blnValidToMin) Then
        '            blnValidToDateTime = False
        '        End If
        '    Else
        '        blnValidToDateTime = False
        '    End If

        '    If Not blnValidToDateTime Then
        '        lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00017"))
        '        Me.imgPeriodToError.Visible = True
        '    End If

        'End If

        '' Validate: Input To Future Date

        'If blnInputToDateTime AndAlso blnValidToDateTime Then
        '    ' Validate: Input to is Future date
        '    Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
        '    dtmInputToDate = Convert.ToDateTime(dtmInputToDate.ToString("dd-MMM-yyyy") + " " + Me.txtToTime.Text.Trim())

        '    If dtmInputToDate > DateTime.Now Then
        '        lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00019"))
        '        Me.imgPeriodToError.Visible = True
        '    End If

        '    Dim udtComFunct As New Common.ComFunction.GeneralFunction()
        '    Dim strValue1 As String = String.Empty
        '    Dim strValue2 As String = String.Empty
        '    udtComFunct.getSystemParameter("HSIVClaimReportMinTime", strValue1, strValue2)
        '    If strValue1.Trim() <> "" Then
        '        If IsDate(udtFormatter.convertDate(strValue1, "E")) Then
        '            Dim dtmReportMinDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))

        '            ' Validate: Input To < Min date
        '            If dtmInputToDate <= dtmReportMinDate Then
        '                'Me.udcInfoMessageBox.AddMessage("010701", "I", "00001", New String() {"%s"}, New String() {strFileName.Trim()})
        '                lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00021"))
        '                Me.imgPeriodToError.Visible = True
        '            End If
        '        End If
        '    End If

        '    ' Validate: Input From > Input To
        '    If blnInputFromDateTime AndAlso blnValidFromDateTime Then
        '        Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
        '        dtmInputFromDate = Convert.ToDateTime(dtmInputFromDate.ToString("dd-MMM-yyyy") + " " + Me.txtFromTime.Text.Trim())

        '        If dtmInputFromDate > dtmInputToDate Then
        '            lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00018"))

        '            Me.imgPeriodFromError.Visible = True
        '            Me.imgPeriodToError.Visible = True
        '        End If

        '    End If
        'End If

        'Return lstError

        Return Nothing
    End Function

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As System.Collections.Generic.List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam As System.Collections.Generic.List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        Me.imgPeriodFromError.Visible = False
        Me.imgPeriodToError.Visible = False

        Dim udtFormatter As New Common.Format.Formatter()
        Me.txtPeriodFrom.Text = udtFormatter.formatInputDate(Me.txtPeriodFrom.Text)
        Me.txtPeriodTo.Text = udtFormatter.formatInputDate(Me.txtPeriodTo.Text)


        lstError = New List(Of Common.ComObject.SystemMessage)
        lstErrorParam = New List(Of String)

        ' Init
        Dim blnInputFromDateTime As Boolean = True
        Dim blnInputToDateTime As Boolean = True
        Dim blnValidFromDateTime As Boolean = True
        Dim blnValidToDateTime As Boolean = True


        ' validation: Input From
        If Me.txtFromTime.Text = "" AndAlso Me.txtPeriodFrom.Text = "" Then
            blnInputFromDateTime = False
            'lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00014"))
            'Me.imgPeriodFromError.Visible = True
        Else
            blnInputFromDateTime = True
        End If

        ' validation: Input To
        If Me.txtToTime.Text = "" AndAlso Me.txtPeriodTo.Text = "" Then
            blnInputToDateTime = False
            lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00015"))
            lstErrorParam.Add(String.Empty)
            Me.imgPeriodToError.Visible = True
        Else
            blnInputToDateTime = True
        End If

        Dim intDummy As Integer

        ' validation: Input From format
        If blnInputFromDateTime Then
            If Me.txtPeriodFrom.Text <> "" AndAlso Me.txtFromTime.Text <> "" AndAlso Me.txtFromTime.Text.Trim().Length >= 5 Then
                Dim blnValidFromDate As Boolean = IsDate(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
                Dim blnValidFromHour As Boolean = Integer.TryParse(Me.txtFromTime.Text.Substring(0, 2).Trim(), intDummy)
                Dim blnValidFromMin As Boolean = Integer.TryParse(Me.txtFromTime.Text.Substring(3, 2).Trim(), intDummy)

                If blnValidFromDate Then
                    Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
                    If dtmInputFromDate.Year < 1900 Or dtmInputFromDate > Date.MaxValue Then
                        blnValidFromDate = False
                    End If
                End If

                ' Hour
                If blnValidFromHour Then
                    Dim intHour As Integer = Convert.ToInt32(Me.txtFromTime.Text.Substring(0, 2).Trim())
                    If intHour < 0 OrElse intHour >= 24 Then
                        blnValidFromHour = False
                    End If
                End If
                ' Min
                If blnValidFromMin Then
                    Dim intMin As Integer = Convert.ToInt32(Me.txtFromTime.Text.Substring(3, 2).Trim())
                    If intMin < 0 OrElse intMin >= 60 Then
                        blnValidFromHour = False
                    End If
                End If

                If Not (blnValidFromDate AndAlso blnValidFromHour AndAlso blnValidFromMin) Then
                    blnValidFromDateTime = False
                End If
            Else
                blnValidFromDateTime = False
            End If

            If Not blnValidFromDateTime Then
                lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00016"))
                lstErrorParam.Add(String.Empty)
                Me.imgPeriodFromError.Visible = True
            End If
        End If

        ' validation: Input To format
        If blnInputToDateTime Then
            If Me.txtToTime.Text.Trim() <> "" AndAlso Me.txtToTime.Text.Trim() <> "" AndAlso Me.txtToTime.Text.Trim().Length >= 5 Then
                Dim blnValidToDate As Boolean = IsDate(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
                Dim blnValidToHour As Boolean = Integer.TryParse(Me.txtToTime.Text.Substring(0, 2).Trim(), intDummy)
                Dim blnValidToMin As Boolean = Integer.TryParse(Me.txtToTime.Text.Substring(3, 2).Trim(), intDummy)


                If blnValidToDate Then
                    Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
                    If dtmInputToDate.Year < 1900 Or dtmInputToDate > Date.MaxValue Then
                        blnValidToDate = False
                    End If
                End If

                ' Hour
                If blnValidToHour Then
                    Dim intHour As Integer = Convert.ToInt32(Me.txtToTime.Text.Substring(0, 2).Trim())
                    If intHour < 0 OrElse intHour >= 24 Then
                        blnValidToHour = False
                    End If
                End If

                ' Min
                If blnValidToMin Then
                    Dim intMin As Integer = Convert.ToInt32(Me.txtToTime.Text.Substring(3, 2).Trim())
                    If intMin < 0 OrElse intMin >= 60 Then
                        blnValidToMin = False
                    End If
                End If

                If Not (blnValidToDate AndAlso blnValidToHour AndAlso blnValidToMin) Then
                    blnValidToDateTime = False
                End If
            Else
                blnValidToDateTime = False
            End If

            If Not blnValidToDateTime Then
                lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00017"))
                lstErrorParam.Add(String.Empty)
                Me.imgPeriodToError.Visible = True
            End If

        End If


        Dim udtComFunct As New Common.ComFunction.GeneralFunction()
        Dim strValue1 As String = String.Empty
        Dim strValue2 As String = String.Empty

        If blnInputFromDateTime AndAlso blnValidFromDateTime Then
            Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
            dtmInputFromDate = Convert.ToDateTime(dtmInputFromDate.ToString("dd-MMM-yyyy") + " " + Me.txtFromTime.Text.Trim())

            udtComFunct.getSystemParameter("HSIVClaimReportMinTime", strValue1, strValue2)
            If strValue1.Trim() <> "" Then
                If IsDate(strValue1) Then
                    Dim dtmReportMinDate As DateTime = Convert.ToDateTime(strValue1)

                    ' Validate: Input From < Min date
                    If dtmInputFromDate < dtmReportMinDate Then
                        lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00020"))
                        lstErrorParam.Add(udtFormatter.formatDateTime(dtmReportMinDate, "E"))
                        Me.imgPeriodFromError.Visible = True
                    End If
                End If
            End If
        End If



        ' Validate: Input To Future Date

        If blnInputToDateTime AndAlso blnValidToDateTime Then
            ' Validate: Input to is Future date
            Dim dtmInputToDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E"))
            dtmInputToDate = Convert.ToDateTime(dtmInputToDate.ToString("dd-MMM-yyyy") + " " + Me.txtToTime.Text.Trim())

            If dtmInputToDate > DateTime.Now Then
                lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00019"))
                lstErrorParam.Add(String.Empty)
                Me.imgPeriodToError.Visible = True
            End If

            udtComFunct.getSystemParameter("HSIVClaimReportMinTime", strValue1, strValue2)
            If strValue1.Trim() <> "" Then
                If IsDate(strValue1) Then
                    Dim dtmReportMinDate As DateTime = Convert.ToDateTime(strValue1)

                    ' Validate: Input To < Min date
                    If dtmInputToDate < dtmReportMinDate Then
                        lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00021"))
                        lstErrorParam.Add(udtFormatter.formatDateTime(dtmReportMinDate, "E"))
                        Me.imgPeriodToError.Visible = True
                    End If
                End If
            End If

            ' Validate: Input From > Input To
            If blnInputFromDateTime AndAlso blnValidFromDateTime Then
                Dim dtmInputFromDate As DateTime = Convert.ToDateTime(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E"))
                dtmInputFromDate = Convert.ToDateTime(dtmInputFromDate.ToString("dd-MMM-yyyy") + " " + Me.txtFromTime.Text.Trim())

                If dtmInputFromDate > dtmInputToDate Then
                    lstError.Add(New Common.ComObject.SystemMessage("010701", "E", "00018"))
                    lstErrorParam.Add(String.Empty)

                    Me.imgPeriodFromError.Visible = True
                    Me.imgPeriodToError.Visible = True
                End If

            End If
        End If

        'Return lstError
    End Sub
End Class