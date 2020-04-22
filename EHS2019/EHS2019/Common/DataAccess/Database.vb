Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports Common.Component
Imports Common.EDSymmetricNamespace

Namespace DataAccess

    Public Class Database
        Implements IDisposable
        ' Track whether Dispose has been called.
        Private disposed As Boolean = False
        ' connection to data source
        Private con As SqlConnection
        Private tx As SqlTransaction = Nothing
        Private begintran As Boolean = False
        Public retrycounter As Integer = 0
        Private isolationLevel As System.Data.IsolationLevel

        Private _DBFlag As String
        Private _Conn As String

        'Public Property DBFlag() As String
        '    Get
        '        Return _DBFlag
        '    End Get
        '    Set(ByVal value As String)
        '        _DBFlag = value
        '    End Set
        'End Property

        Public Property RetryCount() As Integer
            Get
                Return retrycounter
            End Get
            Set(ByVal Value As Integer)
                retrycounter = Value
            End Set
        End Property

        Private _intConnTimeOut As Integer = 0
        Private _intCmdTimeOut As Integer = 0

        Public Property ConnetionTimeout() As Integer
            Get
                Return _intConnTimeOut
            End Get
            Set(ByVal Value As Integer)
                _intConnTimeOut = Value
            End Set
        End Property

        Public Property CommandTimeout() As Integer
            Get
                Return _intCmdTimeOut
            End Get
            Set(ByVal Value As Integer)
                _intCmdTimeOut = Value
            End Set
        End Property

        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <returns>Stored procedure return value.</returns>
        Public Function RunProc(ByVal procName As String) As Integer
            Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
            cmd.ExecuteNonQuery()
            If tx Is Nothing Then Me.Close()
            Return CType(cmd.Parameters("ReturnValue").Value, Integer)

        End Function

        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="prams">Stored procedure params.</param>
        ''' <returns>Stored procedure return value.</returns>
        Public Function RunProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
            Dim cmd As SqlCommand = CreateCommand(procName, prams)
            cmd.ExecuteNonQuery()
            If tx Is Nothing Then Me.Close()
            Return CType(cmd.Parameters("ReturnValue").Value, Integer)
        End Function

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="dataReader">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByRef dataReader As SqlDataReader)
        '    Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        '    dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        'End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        '<summary>
        'Run stored procedure.
        '</summary>
        '<param name="procName">Name of stored procedure.</param>
        '<param name="dataSet">Return result of procedure.</param>
        Public Sub RunProc(ByVal procName As String, ByRef dataSet As DataSet)
            Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dataSet)
            cmd.Dispose()
            da.Dispose()
            If tx Is Nothing Then Me.Close()
        End Sub

        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="dataTable">Return result of procedure.</param>
        Public Sub RunProc(ByVal procName As String, ByRef dataTable As DataTable)
            Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dataTable)
            cmd.Dispose()
            da.Dispose()
            If tx Is Nothing Then Me.Close()
        End Sub


        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="prams">Stored procedure params.</param>
        '''' <param name="dataReader">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataReader As SqlDataReader)
        '    Dim cmd As SqlCommand = CreateCommand(procName, prams)
        '    dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        'End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]


        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="prams">Stored procedure params.</param>
        ''' <param name="dataSet">Return result of procedure.</param>
        ''' <param name="srcTable">source table name.</param>
        Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataSet As DataSet, ByVal srcTable As String)
            Dim cmd As SqlCommand = CreateCommand(procName, prams)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dataSet, srcTable)
            cmd.Dispose()
            da.Dispose()
            If tx Is Nothing Then Me.Close()
        End Sub

        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="prams">Stored procedure params.</param>
        ''' <param name="dataSet">Return result of procedure.</param>
        Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataSet As DataSet)
            Dim cmd As SqlCommand = CreateCommand(procName, prams)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dataSet)
            cmd.Dispose()
            da.Dispose()
            If tx Is Nothing Then Me.Close()
        End Sub

        ''' <summary>
        ''' Run stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="prams">Stored procedure params.</param>
        ''' <param name="dataTable">Return result of procedure.</param>
        Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataTable As DataTable)
            Dim cmd As SqlCommand = CreateCommand(procName, prams)
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dataTable)
            cmd.Dispose()
            da.Dispose()
            If tx Is Nothing Then Me.Close()
        End Sub

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Private Function CreateCommand(ByVal procName As String, ByVal prams() As SqlParameter, ByRef conn As SqlConnection) As SqlCommand
        '    Dim cmd As SqlCommand
        '    cmd = New SqlCommand(procName, conn)
        '    cmd.CommandType = CommandType.StoredProcedure
        '    If GetCmdTimeout() <> 0 Then
        '        cmd.CommandTimeout = GetCmdTimeout()
        '    End If
        '    If Not prams Is Nothing Then
        '        Dim parameter As SqlParameter
        '        For Each parameter In prams
        '            If Not parameter Is Nothing Then cmd.Parameters.Add(parameter)
        '        Next
        '    End If
        '    cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, False, 0, 0, String.Empty, DataRowVersion.Default, Nothing))
        '    Return cmd
        'End Function

        'Public Sub RunProc(ByVal procName1 As String, ByVal procName2 As String)
        '    Using transScope As New TransactionScope()
        '        Using con1 As New SqlConnection(GetConnString)
        '            Dim cmd1 As SqlCommand
        '            cmd1 = CreateCommand(procName1, Nothing, con1)
        '            cmd1.ExecuteNonQuery()

        '            _DBFlag = DBFlagStr.DBFlag2
        '            Using con2 As New SqlConnection(GetConnString)
        '                Dim cmd2 As SqlCommand
        '                cmd2 = CreateCommand(procName2, Nothing, con2)
        '                cmd2.ExecuteNonQuery()
        '            End Using
        '        End Using

        '        transScope.Complete()
        '    End Using
        'End Sub

        'Public Sub RunProc(ByVal procName1 As String, ByVal prams1() As SqlParameter, ByVal procName2 As String, ByVal prams2() As SqlParameter)
        '    Using transScope As New TransactionScope()
        '        Using con1 As New SqlConnection(GetConnString)
        '            Dim cmd1 As SqlCommand
        '            cmd1 = CreateCommand(procName1, prams1, con1)
        '            cmd1.ExecuteNonQuery()

        '            _DBFlag = DBFlagStr.DBFlag2
        '            Using con2 As New SqlConnection(GetConnString)
        '                Dim cmd2 As SqlCommand
        '                cmd2 = CreateCommand(procName2, prams2, con2)
        '                cmd2.ExecuteNonQuery()
        '            End Using
        '        End Using
        '        transScope.Complete()
        '    End Using

        'End Sub


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="dataReader">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByRef dataReader As SqlDataReader, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        '    dependency = New SqlCacheDependency(cmd)
        '    dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="dataSet">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByRef dataSet As DataSet, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        '    Dim da As New SqlDataAdapter(cmd)
        '    dependency = New SqlCacheDependency(cmd)
        '    da.Fill(dataSet)
        '    cmd.Dispose()
        '    da.Dispose()
        '    If tx Is Nothing Then Me.Close()
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="dataTable">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByRef dataTable As DataTable, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, Nothing)
        '    dependency = New SqlCacheDependency(cmd)
        '    Dim da As New SqlDataAdapter(cmd)
        '    da.Fill(dataTable)
        '    cmd.Dispose()
        '    da.Dispose()
        '    If tx Is Nothing Then Me.Close()
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="prams">Stored procedure params.</param>
        '''' <param name="dataReader">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataReader As SqlDataReader, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, prams)
        '    dependency = New SqlCacheDependency(cmd)
        '    dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="prams">Stored procedure params.</param>
        '''' <param name="dataSet">Return result of procedure.</param>
        '''' <param name="srcTable">source table name.</param>
        'Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataSet As DataSet, ByVal srcTable As String, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, prams)
        '    dependency = New SqlCacheDependency(cmd)
        '    Dim da As New SqlDataAdapter(cmd)
        '    da.Fill(dataSet, srcTable)
        '    cmd.Dispose()
        '    da.Dispose()
        '    If tx Is Nothing Then Me.Close()
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="prams">Stored procedure params.</param>
        '''' <param name="dataSet">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataSet As DataSet, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, prams)
        '    dependency = New SqlCacheDependency(cmd)
        '    Dim da As New SqlDataAdapter(cmd)
        '    da.Fill(dataSet)
        '    cmd.Dispose()
        '    da.Dispose()
        '    If tx Is Nothing Then Me.Close()
        'End Sub

        '''' <summary>
        '''' Run stored procedure.
        '''' </summary>
        '''' <param name="procName">Name of stored procedure.</param>
        '''' <param name="prams">Stored procedure params.</param>
        '''' <param name="dataTable">Return result of procedure.</param>
        'Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataTable As DataTable, ByRef dependency As SqlCacheDependency)
        '    'Me.SqlDependency_Start()
        '    Dim cmd As SqlCommand = CreateCommand(procName, prams)
        '    dependency = New SqlCacheDependency(cmd)
        '    Dim da As New SqlDataAdapter(cmd)
        '    da.Fill(dataTable)
        '    cmd.Dispose()
        '    da.Dispose()
        '    If tx Is Nothing Then Me.Close()
        'End Sub

        'Public Function SqlDependency_Start() As Boolean
        '    Return SqlDependency.Start(Me.GetConnString)
        'End Function

        'Public Function SqlDependency_Stop() As Boolean
        '    Return SqlDependency.Stop(Me.GetConnString)
        'End Function

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        ''' <summary>
        ''' Create command object used to call stored procedure.
        ''' </summary>
        ''' <param name="procName">Name of stored procedure.</param>
        ''' <param name="prams">Params to stored procedure.</param>
        ''' <returns>Command object.</returns>
        Private Function CreateCommand(ByVal procName As String, ByVal prams() As SqlParameter) As SqlCommand
            ' make sure connection is open
            Open()
            Dim cmd As SqlCommand
            If tx Is Nothing Then
                cmd = New SqlCommand(procName, con)
            Else
                cmd = New SqlCommand(procName, con, tx)
            End If
            cmd.CommandType = CommandType.StoredProcedure

            ' Add by Anthony
            If GetCmdTimeout() <> 0 Then
                'Overrides default timeout value
                cmd.CommandTimeout = GetCmdTimeout()
            End If
            ' End by Anthony

            ' add proc parameters
            If Not prams Is Nothing Then
                Dim parameter As SqlParameter
                For Each parameter In prams
                    If Not parameter Is Nothing Then cmd.Parameters.Add(parameter)
                Next
            End If

            ' return param
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, False, 0, 0, String.Empty, DataRowVersion.Default, Nothing))

            Return cmd
        End Function

        ''' <summary>
        ''' Get connection string
        ''' </summary>
        ''' <returns>decrypted connection string</returns>
        ''' <remarks>Connection string in web.config was encrypted</remarks>
        Private Function GetConnString() As String

            'Dim strDBFlag As String = ConfigurationManager.AppSettings("DBFlag")
            Dim strDBFlag As String = ConfigurationManager.AppSettings(_DBFlag)

            Dim strConn As String

            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            If Not _Conn Is Nothing Then
                strConn = _Conn
            Else
                strConn = ConfigurationManager.AppSettings(strDBFlag)
            End If
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

            Dim feService As EDSymmetric = New EDSymmetric
            strConn = feService.DecryptData(feService.GetOriginalKey(), strConn)

            If _intConnTimeOut = 0 Then
                ' no change default value
                If ConfigurationManager.AppSettings("ConnectionTimeout") <> "" Then
                    strConn += ";Connection Timeout=" + ConfigurationManager.AppSettings("ConnectionTimeout")
                End If
            Else
                strConn += ";Connection Timeout=" + _intConnTimeOut.ToString
            End If

            'strConn = "data source=hocmissv02; initial catalog=dbEVS_Cache; persist security info=False; user id=EVSappuser; password=cmissql@08; packet size=4096; max pool size=300"

            Return strConn

        End Function


        'Private Function GetConnString2() As String

        '    Dim strDBFlag As String = ConfigurationManager.AppSettings("DBFlag2")
        '    Dim strConn As String = ConfigurationManager.AppSettings(strDBFlag)

        '    Dim feService As EDSymmetric = New EDSymmetric
        '    strConn = feService.DecryptData(feService.GetOriginalKey(), strConn)

        '    If _intConnTimeOut = 0 Then
        '        ' no change default value
        '        If ConfigurationManager.AppSettings("ConnectionTimeout") <> "" Then
        '            strConn += ";Connection Timeout=" + ConfigurationManager.AppSettings("ConnectionTimeout")
        '        End If
        '    Else
        '        strConn += ";Connection Timeout=" + _intConnTimeOut.ToString
        '    End If

        '    'strConn = "data source=hocmissv02; initial catalog=dbEVS_Cache; persist security info=False; user id=EVSappuser; password=cmissql@08; packet size=4096; max pool size=300"

        '    Return strConn

        'End Function

        ''' <summary>
        ''' Get the command timeout value
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetCmdTimeout() As Integer

            Dim intTimeout As Integer = 0

            If _intCmdTimeOut = 0 Then
                'No Change on default value
                If ConfigurationManager.AppSettings("CommandTimeout") <> "" Then
                    intTimeout = Integer.Parse(ConfigurationManager.AppSettings("CommandTimeout"))
                End If
            Else
                ' Override default value
                intTimeout = _intCmdTimeOut
            End If

            Return intTimeout

        End Function

        ''' <summary>
        ''' Open the connection.
        ''' </summary>
        Private Sub Open()
            ' open connection
            If con Is Nothing Then
                con = New SqlConnection(GetConnString)
                con.Open()
                If begintran Then
                    If Not con Is Nothing Then
                        tx = con.BeginTransaction(Me.isolationLevel)
                        begintran = False
                    Else
                        tx = Nothing
                        Return
                    End If
                End If
            Else
                'For closed connection
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                If begintran Then
                    If Not con Is Nothing Then
                        If tx Is Nothing Then
                            tx = con.BeginTransaction(Me.isolationLevel)
                        End If
                    Else
                        tx = Nothing
                        Return
                    End If
                End If
                'End If
            End If
        End Sub

        ''' <summary>
        ''' Begin the Transaction.
        ''' </summary>
        Public Sub BeginTransaction(Optional ByVal iso As System.Data.IsolationLevel = System.Data.IsolationLevel.ReadCommitted)
            Me.isolationLevel = iso
            begintran = True
        End Sub

        ''' <summary>
        ''' Commit the Transaction.
        ''' </summary>
        Public Sub CommitTransaction()
            If Not tx Is Nothing Then
                tx.Commit()
                tx.Dispose()
            End If
            begintran = False
            tx = Nothing
        End Sub

        ''' <summary>
        ''' Rollback the Transaction.
        ''' </summary>
        Public Sub RollBackTranscation()
            If Not tx Is Nothing Then
                tx.Rollback()
                tx.Dispose()
            End If
            begintran = False
            tx = Nothing
        End Sub

        ''' <summary>
        ''' Close the connection.
        ''' </summary>
        Public Sub Close()
            If Not con Is Nothing Then
                'If (con.State <> ConnectionState.Closed) Then
                con.Close()
                'End If
                con = Nothing
            End If
            If Not tx Is Nothing Then
                tx.Dispose()
                tx = Nothing
            End If
            begintran = False
        End Sub

        ''' <summary>
        ''' Release resources.
        ''' </summary>
        ''' <remarks>
        '''  Dispose(disposing As Boolean) executes in two distinct scenarios.
        '''  If disposing is true, the method has been called directly 
        '''  or indirectly by a user's code. Managed and unmanaged resources 
        '''  can be disposed.
        '''  If disposing equals false, the method has been called by the runtime
        '''  from inside the finalizer and you should not reference other    
        '''  objects. Only unmanaged resources can be disposed.
        ''' </remarks>
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            ' Check to see if Dispose has already been called.
            If Not (Me.disposed) Then
                ' If disposing equals true, dispose all managed 
                ' and unmanaged resources.
                If disposing Then
                    ' Free managed objects.

                    ' Release transsaction object
                    If Not tx Is Nothing Then
                        tx.Dispose()
                        tx = Nothing
                    End If

                    ' make sure connection is closed
                    If Not con Is Nothing Then
                        If Not con.State = ConnectionState.Closed Then
                            con.Close()
                        End If
                        con.Dispose()
                        con = Nothing
                    End If
                End If

                ' Release unmanaged resources. If disposing is false,
                ' only the following code is executed.    
            End If

            Me.disposed = True
        End Sub

        ''' <summary>
        ''' Release resources.
        ''' </summary>
        ''' <remarks> Implement IDisposable.
        '''  Do not make this method Overridable.
        '''  A derived class should not be able to override this method.
        ''' </remarks>
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            ' Take yourself off of the finalization queue
            ' to prevent finalization code for this object
            ' from executing a second time.
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Make input param.
        ''' </summary>
        ''' <param name="ParamName">Name of param.</param>
        ''' <param name="DbType">Param type.</param>
        ''' <param name="Size">Param size.</param>
        ''' <param name="Value">Param value.</param>
        ''' <returns>New parameter.</returns>
        Public Function MakeInParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal Value As Object) As SqlParameter
            Return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value)
        End Function

        ''' <summary>
        ''' Make input param.
        ''' </summary>
        ''' <param name="ParamName">Name of param.</param>
        ''' <param name="DbType">Param type.</param>
        ''' <param name="Size">Param size.</param>
        ''' <returns>New parameter.</returns>
        Public Function MakeOutParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer) As SqlParameter
            Return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, Nothing)
        End Function

        ''' <summary>
        ''' Make stored procedure param.
        ''' </summary>
        ''' <param name="ParamName">Name of param.</param>
        ''' <param name="DbType">Param type.</param>
        ''' <param name="Size">Param size.</param>
        ''' <param name="Direction">Parm direction.</param>
        ''' <param name="Value">Param value.</param>
        ''' <returns>New parameter.</returns>
        Public Function MakeParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Int32, ByVal Direction As ParameterDirection, ByVal Value As Object) As SqlParameter
            Dim param As SqlParameter

            If Size > 0 Then
                param = New SqlParameter(ParamName, DbType, Size)
            Else
                param = New SqlParameter(ParamName, DbType)
            End If

            param.Direction = Direction

            If Not Value Is Nothing Then
                If Value Is DBNull.Value Then
                    param.Value = DBNull.Value
                Else
                    If Not (Direction = ParameterDirection.Output) Then
                        param.Value = Value
                    End If
                End If

            End If

            Return param
        End Function

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Sub New()
            _DBFlag = DBFlagStr.DBFlag
            _Conn = Nothing
        End Sub

        Public Sub New(ByVal strDBFlag As String)
            _DBFlag = strDBFlag
            _Conn = Nothing
        End Sub

        Public Sub New(ByVal strDBFlag As String, ByVal strConn As String)
            Me.New()
            _DBFlag = strDBFlag
            _Conn = strConn
        End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        ''' <remarks> This Finalize method will run only if the 
        ''' Dispose method does not get called.
        ''' By default, methods are NotOverridable. 
        ''' This prevents a derived class from overriding this method. </remarks>
        Protected Overrides Sub Finalize()
            ' Do not re-create Dispose clean-up code here.
            ' Calling Dispose(false) is optimal in terms of
            ' readability and maintainability.
            Dispose(False)
        End Sub

    End Class
End Namespace

