'' ---------------------------------------------------------------------
'' Version           : 1.0.0
'' Date Created      : 01-Jun-2010
'' Create By         : Pak Ho LEE
'' Remark            : Convert from C# to VB with AI3's SSO source code.
''
'' Type              : Data Access 
''
'' ---------------------------------------------------------------------
'' Change History    :
'' ID     REF NO             DATE                WHO                                       DETAIL
'' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
''
'' ---------------------------------------------------------------------

'Imports System
'Imports System.Data
'Imports System.Data.Odbc
'Imports System.IO
'Imports System.Collections
'Imports System.Text

'Public Class DAL

'    Public Sub New()
'    End Sub

'    ' <ID018 Start> 
'    'umail
'    Public Shared Function odbc_executeDataSetSQL(ByVal strSQL As String) As DataSet
'        Dim objConnection As OdbcConnection = Nothing
'        Dim ds As New DataSet()
'        Try
'            objConnection = odbc_getConnection()

'            If objConnection Is Nothing Then
'                Return Nothing
'            End If
'            ' end auto-db switching 

'            Dim sql As String = strSQL
'            Dim dc As New OdbcCommand(sql, objConnection)
'            dc.CommandTimeout = Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("DBCommandTimeout"))
'            dc.CommandType = CommandType.Text

'            Dim da As New OdbcDataAdapter(dc)

'            da.Fill(ds)
'        Catch objEx As Exception
'            Throw objEx
'        Finally
'            If objConnection IsNot Nothing AndAlso objConnection.State = ConnectionState.Open Then
'                objConnection.Close()

'            End If
'        End Try

'        Return ds
'    End Function
'    ' <ID018 End> 

'    Public Shared Function odbc_executeScalarSP(ByVal commandText As String, ByVal commandParameters As OdbcParameter()) As Integer

'        Dim noOfRowAffected As Integer = 0
'        Dim objConnection As OdbcConnection = Nothing
'        Try
'            ' auto-db switching 

'            objConnection = odbc_getConnection()
'            If objConnection Is Nothing Then
'                Return -1
'            End If
'            ' end auto-db switching 

'            Dim sql As String = commandText
'            Dim dc As New OdbcCommand(sql, objConnection)
'            dc.CommandType = CommandType.StoredProcedure
'            For Each p As OdbcParameter In commandParameters
'                dc.Parameters.Add(p)
'            Next

'            noOfRowAffected = CInt(dc.ExecuteScalar())
'        Catch ex As Exception
'            Throw ex
'        Finally
'            If objConnection IsNot Nothing AndAlso objConnection.State = ConnectionState.Open Then
'                objConnection.Close()
'            End If
'        End Try

'        Return noOfRowAffected
'    End Function

'    Public Shared Function odbc_getConnection() As OdbcConnection
'        Dim slDBSetting As SortedList = DirectCast(System.Web.HttpContext.Current.Cache("cachedDBSetting"), SortedList)
'        'new SortedList();
'        Dim dbfile As String = System.Configuration.ConfigurationManager.AppSettings("dbsetting")
'        If slDBSetting Is Nothing Then
'            If dbfile IsNot Nothing Then
'                Try
'                    slDBSetting = New SortedList()
'                    Dim input As [String]
'                    Dim sr As StreamReader = File.OpenText(dbfile)
'                    While (InlineAssignHelper(input, sr.ReadLine())) IsNot Nothing
'                        slDBSetting.Add(input.Split("|"c)(0), input.Split("|"c)(1))
'                    End While
'                    sr.Close()
'                Catch ex As Exception
'                    Dim a As String = ex.Message
'                End Try
'            End If
'            If slDBSetting IsNot Nothing Then
'                System.Web.HttpContext.Current.Cache.Insert("cachedDBSetting", slDBSetting, Nothing, DateTime.Now.AddSeconds(30), TimeSpan.Zero)
'            End If
'        End If

'        Dim dbMode As Integer = Integer.Parse(slDBSetting("DBMode").ToString())
'        Dim DSNcurr As String = slDBSetting("DSNcurr").ToString()
'        Dim DSNsb As String = slDBSetting("DSNsb").ToString()
'        If dbMode = 1 Then
'            Return Nothing
'        End If
'        ' if disconnected mode (1), do nothing and return
'        Dim objConnection As OdbcConnection = Nothing
'        If dbMode = 0 Then
'            ' if manual mode, leave it as usual 
'            objConnection = New OdbcConnection(System.Configuration.ConfigurationManager.AppSettings("DSNConnString"))
'            objConnection.Open()
'        ElseIf dbMode = 2 Then
'            ' if auto mode, then try the 2nd connection
'            objConnection = New OdbcConnection(slDBSetting(DSNcurr).ToString())
'            Dim isFailed As Boolean = False
'            Try
'                objConnection.Open()
'            Catch generatedExceptionName As Exception
'                isFailed = True
'            End Try
'            If isFailed Then
'                objConnection = New OdbcConnection(slDBSetting(DSNsb).ToString())
'                objConnection.Open()

'                Dim dbSettingText As String = ""
'                dbSettingText = dbSettingText + "DBMode" + "|" + slDBSetting("DBMode") + vbCr & vbLf
'                dbSettingText = dbSettingText + "DSNcurr" + "|" + slDBSetting("DSNsb") + vbCr & vbLf
'                dbSettingText = dbSettingText + "DSNsb" + "|" + slDBSetting("DSNcurr") + vbCr & vbLf
'                dbSettingText = dbSettingText + "DSNConnString" + "|" + slDBSetting("DSNConnString") + vbCr & vbLf
'                dbSettingText = dbSettingText + "DSNConnString2" + "|" + slDBSetting("DSNConnString2") + vbCr & vbLf

'                Try
'                    Dim sr As StreamWriter = File.CreateText(dbfile)
'                    sr.Write(dbSettingText)
'                    sr.Close()
'                Catch generatedExceptionName As Exception
'                End Try
'            End If
'        End If
'        Return objConnection
'    End Function

'    'process null value in DB query
'    Public Shared Function DBCheckNull(ByVal objDBValue As Object, ByVal objNullReplacedValue As Object) As Object

'        If objDBValue Is Nothing OrElse IsDBNull(objDBValue) Then
'            Return objNullReplacedValue
'        End If
'        Return objDBValue

'    End Function

'    Public Shared Function DBCheckNull(ByVal objDBValue As Object) As Object
'        Return DBCheckNull(objDBValue, "")
'    End Function

'    Public Shared Function odbc_executeDataSetSP(ByVal commandText As String, ByVal commandParameters As OdbcParameter()) As DataSet
'        Dim objConnection As OdbcConnection = Nothing
'        Dim sql As String = commandText
'        Dim ds As New DataSet()
'        Try
'            ' auto-db switching 

'            ' if disconnected mode (1), do nothing and return
'            objConnection = odbc_getConnection()

'            If objConnection Is Nothing Then
'                Return Nothing
'            End If
'            ' end auto-db switching 

'            Dim dc As New OdbcCommand(sql, objConnection)
'            dc.CommandTimeout = Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("DBCommandTimeout"))
'            dc.CommandType = CommandType.StoredProcedure
'            For Each p As OdbcParameter In commandParameters
'                dc.Parameters.Add(p)
'            Next
'            Dim da As New OdbcDataAdapter(dc)

'            da.Fill(ds)
'        Catch objEx As Exception

'            Throw objEx
'        Finally
'            If objConnection IsNot Nothing AndAlso objConnection.State = ConnectionState.Open Then
'                objConnection.Close()

'            End If
'        End Try

'        Return ds
'    End Function

'    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
'        target = value
'        Return value
'    End Function
'End Class
