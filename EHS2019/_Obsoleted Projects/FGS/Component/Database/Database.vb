Imports System.Data
Imports System.Data.SqlClient

Public Class Database
    Implements IDisposable

#Region "Field"

    Private _strDBFlag As String
    Private _intConnTimeOut As Integer = 0
    Private _intCmdTimeOut As Integer = 0
    Private _intRetryCount As Integer = 0
    Private _blnDisposed As Boolean = False
    Private _blnBeginTran As Boolean = False
    Private _connection As SqlConnection
    Private _transaction As SqlTransaction = Nothing
    Private _isolationLevel As IsolationLevel

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Not accessible, must specify the DB Flag on New
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        ' Unaccessable
    End Sub

    Public Sub New(ByVal strDBFlag As String)
        _strDBFlag = strDBFlag
    End Sub

#End Region

#Region "Function: Construct parameter"

    ''' <summary>
    ''' Make input parameter
    ''' </summary>
    ''' <param name="ParamName">Name</param>
    ''' <param name="DbType">Data type</param>
    ''' <param name="Size">Size</param>
    ''' <param name="Value">Value</param>
    ''' <returns>New parameter</returns>
    Public Function MakeInParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal Value As Object) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value)
    End Function

    ''' <summary>
    ''' Make output parameter
    ''' </summary>
    ''' <param name="ParamName">Name</param>
    ''' <param name="DbType">Data type</param>
    ''' <param name="Size">Size</param>
    ''' <returns>New parameter</returns>
    Public Function MakeOutParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, Nothing)
    End Function

    Private Function MakeParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Int32, ByVal Direction As ParameterDirection, ByVal Value As Object) As SqlParameter
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

#End Region

#Region "Function: Run stored procedure"

    ''' <summary>
    ''' Run stored procedure
    ''' </summary>
    ''' <param name="procName">Name of stored procedure</param>
    ''' <param name="prams">Stored procedure parameters, pass Nothing if no parameters</param>
    Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter)
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        If _transaction Is Nothing Then Me.Close()
    End Sub

    ''' <summary>
    ''' Run stored procedure (data table)
    ''' </summary>
    ''' <param name="procName">Name of stored procedure</param>
    ''' <param name="prams">Stored procedure parameters, pass Nothing if no parameters</param>
    ''' <param name="dt">Return result of stored procedure in data table</param>
    Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dt As DataTable)
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)
        cmd.Dispose()
        da.Dispose()
        If _transaction Is Nothing Then Me.Close()
    End Sub

    ''' <summary>
    ''' Run stored procedure (data set)
    ''' </summary>
    ''' <param name="procName">Name of stored procedure</param>
    ''' <param name="prams">Stored procedure parameters</param>
    ''' <param name="ds">Return result of stored procedure in data set</param>
    Public Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef ds As DataSet)
        Dim cmd As SqlCommand = CreateCommand(procName, prams)
        Dim da As New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()
        da.Dispose()
        If _transaction Is Nothing Then Me.Close()
    End Sub

    ''' <summary>
    ''' Create command object used to call stored procedure
    ''' </summary>
    ''' <param name="procName">Name of stored procedure</param>
    ''' <param name="prams">Params to stored procedure</param>
    ''' <returns>Command object</returns>
    Private Function CreateCommand(ByVal procName As String, ByVal prams() As SqlParameter) As SqlCommand
        ' make sure connection is open
        Open()
        Dim cmd As SqlCommand
        If _transaction Is Nothing Then
            cmd = New SqlCommand(procName, _connection)
        Else
            cmd = New SqlCommand(procName, _connection, _transaction)
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
    ''' Open the connection
    ''' </summary>
    Private Sub Open()
        ' open connection
        If _connection Is Nothing Then
            _connection = New SqlConnection(GetConnString)
            _connection.Open()
            If _blnBeginTran Then
                If Not _connection Is Nothing Then
                    _transaction = _connection.BeginTransaction(Me._isolationLevel)
                    _blnBeginTran = False
                Else
                    _transaction = Nothing
                    Return
                End If
            End If
        Else
            'For closed connection
            If _connection.State = ConnectionState.Closed Then
                _connection.Open()
            End If
            If _blnBeginTran Then
                If Not _connection Is Nothing Then
                    If _transaction Is Nothing Then
                        _transaction = _connection.BeginTransaction(Me._isolationLevel)
                    End If
                Else
                    _transaction = Nothing
                    Return
                End If
            End If
            'End If
        End If
    End Sub

    ''' <summary>
    ''' Close the connection.
    ''' </summary>
    Private Sub Close()
        If Not _connection Is Nothing Then
            _connection.Close()
            _connection = Nothing
        End If
        If Not _transaction Is Nothing Then
            _transaction.Dispose()
            _transaction = Nothing
        End If
        _blnBeginTran = False
    End Sub

#End Region

#Region "Function: Begin/End transaction"

    ''' <summary>
    ''' Begin the Transaction
    ''' </summary>
    Public Sub BeginTransaction(Optional ByVal iso As System.Data.IsolationLevel = System.Data.IsolationLevel.ReadCommitted)
        _isolationLevel = iso
        _blnBeginTran = True
    End Sub

    ''' <summary>
    ''' Commit the Transaction
    ''' </summary>
    Public Sub CommitTransaction()
        If Not _transaction Is Nothing Then
            _transaction.Commit()
            _transaction.Dispose()
        End If
        _blnBeginTran = False
        _transaction = Nothing
    End Sub

    ''' <summary>
    ''' Rollback the Transaction
    ''' </summary>
    Public Sub RollBackTranscation()
        If Not _transaction Is Nothing Then
            _transaction.Rollback()
            _transaction.Dispose()
        End If
        _blnBeginTran = False
        _transaction = Nothing
    End Sub

#End Region

#Region "Supporting function"

    ''' <summary>
    ''' Get connection string
    ''' </summary>
    ''' <returns>decrypted connection string</returns>
    ''' <remarks>Connection string in web.config was encrypted</remarks>
    Private Function GetConnString() As String
        Dim strConn As String = ConfigurationManager.AppSettings(ConfigurationManager.AppSettings(_strDBFlag))

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

        Return strConn

    End Function

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
    ''' Release resources
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
        If Not (Me._blnDisposed) Then
            ' If disposing equals true, dispose all managed 
            ' and unmanaged resources.
            If disposing Then
                ' Free managed objects.

                ' Release transsaction object
                If Not _transaction Is Nothing Then
                    _transaction.Dispose()
                    _transaction = Nothing
                End If

                ' make sure connection is closed
                If Not _connection Is Nothing Then
                    If Not _connection.State = ConnectionState.Closed Then
                        _connection.Close()
                    End If
                    _connection.Dispose()
                    _connection = Nothing
                End If
            End If

            ' Release unmanaged resources. If disposing is false,
            ' only the following code is executed.    
        End If

        Me._blnDisposed = True
    End Sub

    ''' <summary>
    ''' Release resources
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

#End Region

#Region "Property"

    Public Property RetryCount() As Integer
        Get
            Return _intRetryCount
        End Get
        Set(ByVal Value As Integer)
            _intRetryCount = Value
        End Set
    End Property

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

#End Region

End Class
