Imports System.Configuration
Imports System.IO
Imports System.Xml.Serialization

Public Class Form1

#Region "Page Events"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitControlOnce()

    End Sub

    Private Sub InitControlOnce()
        Dim objStreamReader As New StreamReader(ConfigurationManager.AppSettings("LogListPath"))

        Dim udtXmlLogList As New XmlLogModelCollection()

        udtXmlLogList = (New XmlSerializer(udtXmlLogList.GetType)).Deserialize(objStreamReader)
        objStreamReader.Close()

        _udtLogList = New LogModelCollection(udtXmlLogList)

        If ConfigurationManager.AppSettings("SecondsToWait") <> "" Then
            intSecondsToWait = Integer.Parse(ConfigurationManager.AppSettings("SecondsToWait"))
        End If

        ' Event Source
        ddlEventSource.Items.Add("--- Please Select ---")
        ddlEventSource.Items.AddRange(_udtLogList.LogList.Keys.ToArray)
        ddlEventSource.SelectedIndex = 0

        ' Event ID
        'ddlEventID.Items.Add("--- Please Select ---")
        'ddlEventID.SelectedIndex = 0

        ' Severity
        lblSeverity.Text = String.Empty

        ' Write Log
        btnWriteLog.Enabled = False

        WriteConsole("Program starts")

    End Sub

#End Region

#Region "Global Variables"

    Private _udtLogList As LogModelCollection
    Private dtstartTime As DateTime
    Private intSecondsToWait As Integer
    Private blnWaiting As Boolean = False
#End Region

#Region "Classes"

    <XmlRoot("LogList")> _
    Public Class XmlLogModelCollection

        <XmlElementAttribute(ElementName:="Log")> _
        Public LogList As List(Of XmlLogModel)

    End Class

    Public Class XmlLogModel

        Public EventSource As String
        Public EventID As String
        Public Severity As String
        Public Message As String

    End Class

    '

    Public Class LogModelCollection

        Public LogList As Dictionary(Of String, List(Of LogModel))

        Private Sub New()
            LogList = New Dictionary(Of String, List(Of LogModel))
        End Sub

        Public Sub New(ByVal udtXmlLogList As XmlLogModelCollection)
            Me.New()

            For Each udtXmlLog As XmlLogModel In udtXmlLogList.LogList
                Dim udtLog As New LogModel
                udtLog.EventID = udtXmlLog.EventID
                udtLog.Severity = udtXmlLog.Severity
                udtLog.Message = udtXmlLog.Message

                If LogList.ContainsKey(udtXmlLog.EventSource) = False Then
                    LogList.Add(udtXmlLog.EventSource, New List(Of LogModel))
                End If

                LogList(udtXmlLog.EventSource).Add(udtLog)

            Next

        End Sub

        Public Function Find(ByVal strEventSource As String, ByVal strEventID As String)
            For Each udtLog As LogModel In Me.LogList(strEventSource)
                If udtLog.EventID = strEventID Then
                    Return udtLog
                End If
            Next

            Return Nothing

        End Function

    End Class

    Public Class LogModel

        Public EventID As String
        Public Severity As String
        Public Message As String

    End Class


#End Region

    Private Sub ddlEventSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEventSource.SelectedIndexChanged
        btnWriteLog.Enabled = False

        ' Event ID
        ddlEventID.Items.Clear()
        ddlEventID.Items.Add("--- Please Select ---")
        ddlEventID.SelectedIndex = 0

        lblSeverity.Text = String.Empty
        txtMessage.Text = String.Empty

        If ddlEventSource.SelectedIndex = 0 Then
            Return
        End If

        Dim udtLogList As List(Of LogModel) = _udtLogList.LogList(ddlEventSource.SelectedItem)

        For Each udtLog As LogModel In udtLogList
            ddlEventID.Items.Add(udtLog.EventID)
        Next

    End Sub

    Private Sub ddlEventID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEventID.SelectedIndexChanged
        lblSeverity.Text = String.Empty
        txtMessage.Text = String.Empty

        If ddlEventID.SelectedIndex = 0 Then
            btnWriteLog.Enabled = False
            Return
        End If

        Dim udtLog As LogModel = _udtLogList.Find(ddlEventSource.SelectedItem, ddlEventID.SelectedItem)

        lblSeverity.Text = udtLog.Severity
        txtMessage.Text = udtLog.Message

        EnableWriteLog()
    End Sub

    '

    Private Sub btnWriteLog_Click(sender As Object, e As EventArgs) Handles btnWriteLog.Click

        btnWriteLog.Enabled = False
        Timer1.Start()
        dtstartTime = DateTime.Now

        WriteEventLog(ddlEventSource.SelectedItem, ddlEventID.SelectedItem, lblSeverity.Text, txtMessage.Text)
        WriteConsole(String.Format("1 log written: EventSource={0}, EventID={1}, Severity={2}, Message={3}", _
                                    ddlEventSource.SelectedItem, _
                                    ddlEventID.SelectedItem, _
                                    lblSeverity.Text, _
                                    txtMessage.Text))



    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim elapsedSeconds As Integer = (DateTime.Now - dtstartTime).TotalSeconds
        Dim remainingSeconds As Integer = intSecondsToWait - elapsedSeconds
        
        If (remainingSeconds <= 0) Then
            Timer1.Stop()
            blnWaiting = False
            btnWriteLog.Text = "Write Log"
            EnableWriteLog()
        Else
            btnWriteLog.Text = String.Format("Write Log in ({0}) ", remainingSeconds)
            blnWaiting = True
        End If
    End Sub

    Private Sub EnableWriteLog()

        If ddlEventSource.SelectedIndex = 0 Then
            Return
        End If

        If ddlEventID.SelectedIndex = 0 Then
            Return
        End If

        If blnWaiting = True Then
            Return
        End If

        btnWriteLog.Enabled = True
    End Sub

    Private Sub WriteEventLog(ByVal strEventSource As String, ByVal strEventID As String, ByVal strEntryType As String, ByVal strMessage As String)
        strMessage = String.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), strMessage)

        Dim typeEntryType As EventLogEntryType = Nothing

        Select Case strEntryType
            Case "Error"
                typeEntryType = EventLogEntryType.Error
            Case "Warning"
                typeEntryType = EventLogEntryType.Warning
            Case Else
                Throw New NotImplementedException
        End Select

        Try
            If Not EventLog.SourceExists(strEventSource) Then EventLog.CreateEventSource(strEventSource, "Application")

            Dim udtEventLog As New EventLog

            udtEventLog.Source = strEventSource

            udtEventLog.WriteEntry(strMessage, typeEntryType, CInt(strEventID))

        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try

    End Sub

    Private Sub WriteConsole(ByVal strText As String)

        txtConsole.AppendText(String.Format("[{0}]  {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strText, Environment.NewLine))
        txtConsole.ScrollToCaret()

    End Sub

End Class
