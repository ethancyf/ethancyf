Imports System.Security.Cryptography
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject
Imports Common.DataAccess
Imports System.Data
Imports System.Data.SqlClient

Namespace ComFunction
    Public Class AccountSecurity

#Region "Enum"
        Public Enum PasswordLevel
            MD5 = 1
            SHA512 = 2
        End Enum

        Public Enum EnumPlatformType
            SP
            VU
            DE
            IVRS
            ICW
        End Enum

        Public Enum EnumVerifyPasswordResult
            Pass
            Incorrect
            RequireUpdate
        End Enum
        Public Enum IVRSFunctCode
            FunctcodeLogin = 990201
        End Enum
        
#End Region

#Region "Variable"
        Private Shared Function GetMinLevel() As Integer
            Return (New GeneralFunction).getSystemParameter("PasswordLevelMin")
        End Function

        Private Shared Function GetMaxLevel() As Integer
            Return (New GeneralFunction).getSystemParameter("PasswordLevelMax")
        End Function

        Private Shared Function GetIVRSBackwardCheck() As String
            Return (New GeneralFunction).getSystemParameter("EnableIVRSPasswordBackwardChecking")
        End Function


        Private Class ColumnList

            Public Class SP
                Public Shared ID1 As String() = New String() {"SP_ID"}
                Public Shared AccountPassword As String() = New String() {"User_Password", "SP_Password"}
                Public Shared AccountPasswordLevel As String() = New String() {"Password_Level", "SP_Password_Level"}
                Public Shared TSMP As String() = New String() {"TSMP"}
            End Class

            Public Class DE
                Public Shared ID1 As String() = New String() {"SP_ID"}
                Public Shared ID2 As String() = New String() {"Data_Entry_Account"}
                Public Shared AccountPassword As String() = New String() {"Data_Entry_Password", "User_Password"}
                Public Shared AccountPasswordLevel As String() = New String() {"Password_Level"}
                Public Shared TSMP As String() = New String() {"TSMP"}
            End Class

            Public Class VU
                Public Shared ID1 As String() = New String() {"User_ID"}
                Public Shared AccountPassword As String() = New String() {"User_Password"}
                Public Shared AccountPasswordLevel As String() = New String() {"Password_Level"}
                Public Shared TSMP As String() = New String() {"TSMP"}
            End Class

            Public Class IVRS
                Public Shared ID1 As String() = New String() {"SP_ID"}
                Public Shared AccountPassword As String() = New String() {"SP_IVRS_Password"}
                Public Shared AccountPasswordLevel As String() = New String() {"IVRS_Password_Level", "SP_IVRS_Password_Level"}
                Public Shared TSMP As String() = New String() {"TSMP"}
                Public Shared OriginalPassword As String() = New String() {"SP_IVRS_Password"}
                Public Shared OriginalPasswordLevel As String() = New String() {"SP_IVRS_Password_Level"}
            End Class

            Public Class ICW
                Public Shared ID1 As String() = New String() {"Staff_ID"}
                Public Shared AccountPassword As String() = New String() {"Staff_Password"}
                Public Shared AccountPasswordLevel As String() = New String() {"Staff_Password_Level"}
            End Class

        End Class
#End Region

#Region "Function"
        Public Shared Function VerifyPassword(ByVal udtType As EnumPlatformType, ByVal udtUserAC As DataTable, ByVal strInputPassword As String, Optional ByRef blnTransitPasswordSuccess As Boolean = Nothing) As VerifyPasswordResultModel
            Dim strID1 As String = String.Empty
            Dim strID2 As String = String.Empty
            Dim strAccountPassword As String = String.Empty
            Dim strAccountPasswordLevel As String = String.Empty
            Dim strIVRSOrginalPassword As String = String.Empty
            Dim strIVRSOrginalPasswordLevel As String = String.Empty
            Dim bytTSMP As Byte() = Nothing
            Dim dtUserAC_OriginalIVRS As DataTable = Nothing
            Dim blnBackwardCheck As Boolean = False

            Dim intMinLevel As Integer = GetMinLevel()
            Dim intMaxLevel As Integer = GetMaxLevel()

            Dim IResult As New VerifyPasswordResultModel()

            Select Case udtType
                Case EnumPlatformType.SP
                    strID1 = GetColumnValue(udtUserAC, ColumnList.SP.ID1, GetType(String))
                    strAccountPassword = GetColumnValue(udtUserAC, ColumnList.SP.AccountPassword, GetType(String))
                    strAccountPasswordLevel = GetColumnValue(udtUserAC, ColumnList.SP.AccountPasswordLevel, GetType(String))
                    bytTSMP = GetColumnValue(udtUserAC, ColumnList.SP.TSMP, GetType(Byte()))

                Case EnumPlatformType.DE
                    strID1 = GetColumnValue(udtUserAC, ColumnList.DE.ID1, GetType(String))
                    strID2 = GetColumnValue(udtUserAC, ColumnList.DE.ID2, GetType(String))
                    strAccountPassword = GetColumnValue(udtUserAC, ColumnList.DE.AccountPassword, GetType(String))
                    strAccountPasswordLevel = GetColumnValue(udtUserAC, ColumnList.DE.AccountPasswordLevel, GetType(String))
                    bytTSMP = GetColumnValue(udtUserAC, ColumnList.DE.TSMP, GetType(Byte()))

                Case EnumPlatformType.VU
                    strID1 = GetColumnValue(udtUserAC, ColumnList.VU.ID1, GetType(String))
                    strAccountPassword = GetColumnValue(udtUserAC, ColumnList.VU.AccountPassword, GetType(String))
                    strAccountPasswordLevel = GetColumnValue(udtUserAC, ColumnList.VU.AccountPasswordLevel, GetType(String))
                    bytTSMP = GetColumnValue(udtUserAC, ColumnList.VU.TSMP, GetType(Byte()))

                Case EnumPlatformType.IVRS
                    strID1 = GetColumnValue(udtUserAC, ColumnList.IVRS.ID1, GetType(String))
                    strAccountPassword = GetColumnValue(udtUserAC, ColumnList.IVRS.AccountPassword, GetType(String))
                    strAccountPasswordLevel = GetColumnValue(udtUserAC, ColumnList.IVRS.AccountPasswordLevel, GetType(String))
                    bytTSMP = GetColumnValue(udtUserAC, ColumnList.IVRS.TSMP, GetType(Byte()))
                    'IVRS Account check
                    If String.IsNullOrEmpty(strAccountPassword) AndAlso String.IsNullOrEmpty(strAccountPasswordLevel) Then
                        Return IResult
                        Exit Function
                    End If
                    blnBackwardCheck = IVRSBackwardCheck()
                    If blnBackwardCheck Then
                        dtUserAC_OriginalIVRS = GetOriginalIVRS(strID1)
                        If Not dtUserAC_OriginalIVRS Is Nothing AndAlso dtUserAC_OriginalIVRS.Rows.Count > 0 Then
                            strIVRSOrginalPassword = GetColumnValue(dtUserAC_OriginalIVRS, ColumnList.IVRS.OriginalPassword, GetType(String))
                            strIVRSOrginalPasswordLevel = GetColumnValue(dtUserAC_OriginalIVRS, ColumnList.IVRS.OriginalPasswordLevel, GetType(String))
                        End If
                    End If
                Case EnumPlatformType.ICW
                    strID1 = GetColumnValue(udtUserAC, ColumnList.ICW.ID1, GetType(String))
                    strAccountPassword = GetColumnValue(udtUserAC, ColumnList.ICW.AccountPassword, GetType(String))
                    strAccountPasswordLevel = GetColumnValue(udtUserAC, ColumnList.ICW.AccountPasswordLevel, GetType(String))
            End Select

            If strAccountPasswordLevel < intMinLevel Then
                IResult.VerifyResult = EnumVerifyPasswordResult.RequireUpdate
            Else
                If strAccountPassword = Hash(strInputPassword, strAccountPasswordLevel) Then
                    IResult.VerifyResult = EnumVerifyPasswordResult.Pass
                    If strAccountPasswordLevel < intMaxLevel Then
                        Dim udtPasswordModel As New TransitPasswordModel(strID1, strID2, strInputPassword, bytTSMP)
                        IResult.TransitPassword = TransitPassword(udtType, udtPasswordModel)
                    End If

                    If udtType = EnumPlatformType.IVRS AndAlso blnBackwardCheck Then
                        If Not dtUserAC_OriginalIVRS Is Nothing AndAlso dtUserAC_OriginalIVRS.Rows.Count > 0 Then
                                UpdateOriginalIVRSStatus(strID1)
                        End If
                    End If
                ElseIf Not dtUserAC_OriginalIVRS Is Nothing AndAlso dtUserAC_OriginalIVRS.Rows.Count > 0 Then
                    If udtType = EnumPlatformType.IVRS AndAlso blnBackwardCheck AndAlso strIVRSOrginalPassword = Hash(strInputPassword, strIVRSOrginalPasswordLevel) Then
                        IResult.VerifyResult = EnumVerifyPasswordResult.Pass
                        ErrorHandler.Log(IVRSFunctCode.FunctcodeLogin, ErrorHandler.SeverityCode, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, "Hash password fail check with " + PasswordLevel.SHA512.ToString + ", success with " + PasswordLevel.MD5.ToString + " , SPID : " + strID1)
                    End If              
                End If
            End If

            Return IResult
        End Function

        Private Shared Function IVRSBackwardCheck() As Boolean

            Dim blnEnable As Boolean = False
            Dim strSysParm As String = String.Empty

            strSysParm = GetIVRSBackwardCheck()

            Select Case strSysParm.Trim
                Case "Y"
                    blnEnable = True
                Case "N", String.Empty
                    blnEnable = False
                Case Else
                    blnEnable = False
            End Select

            Return blnEnable
        End Function


        Public Shared Function VerifyPasswordLevel(ByVal intInputPasswordLevel As Integer) As Boolean
            Dim bln As Boolean = False
            If intInputPasswordLevel <= GetMaxLevel() And intInputPasswordLevel >= GetMinLevel() Then
                bln = True
            End If

            Return bln
        End Function

        Public Shared Function Hash(ByVal strSourceValue As String) As HashModel
            Return New HashModel(Hash(strSourceValue, GetMaxLevel), GetMaxLevel)
        End Function

        Private Shared Function Hash(ByVal strPassword As String, ByVal strLevel As PasswordLevel) As String
            Select Case strLevel
                Case PasswordLevel.SHA512
                    strPassword = SHA512Hash(strPassword)
                Case Else
                    Throw New Exception(String.Format("MaxPasswordLevel: Unexpected value (strLevel={0})", strLevel))
            End Select

            Return strPassword
        End Function

        Private Shared Function TransitPassword(ByVal udtType As EnumPlatformType, ByVal udtPasswordModel As TransitPasswordModel) As Boolean
            Dim blnResult As Boolean = False
            Dim db As New Database

            With udtPasswordModel
                Dim udtHashPasswordModel As HashModel = Hash(.InputPassword)

                Try
                    db.BeginTransaction()
                    Select Case udtType
                        Case EnumPlatformType.SP
                            Dim parms() As SqlParameter = { _
                                db.MakeInParam("@sp_ID", SqlDbType.Char, 8, .ID1), _
                                db.MakeInParam("@sp_password", SqlDbType.VarChar, 100, udtHashPasswordModel.HashedValue), _
                                db.MakeInParam("@sp_password_level", SqlDbType.Int, 4, udtHashPasswordModel.PasswordLevel), _
                                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, .UserACTSMP) _
                            }
                            db.RunProc("proc_HCSPUserAC_upd_Password_Transit", parms)
                            db.CommitTransaction()
                            blnResult = True

                        Case EnumPlatformType.DE
                            Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, .ID1), _
                                db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, .ID2), _
                                db.MakeInParam("@Password", SqlDbType.VarChar, 100, udtHashPasswordModel.HashedValue), _
                                db.MakeInParam("@data_entry_password_level", SqlDbType.Int, 4, udtHashPasswordModel.PasswordLevel), _
                                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, .UserACTSMP) _
                            }
                            db.RunProc("proc_DataEntryUserAC_upd_Password_Transit", parms)
                            db.CommitTransaction()
                            blnResult = True

                        Case EnumPlatformType.VU
                            Dim parms() As SqlParameter = { _
                                db.MakeInParam("@User_ID", SqlDbType.Char, 8, .ID1), _
                                db.MakeInParam("@User_password", SqlDbType.VarChar, 100, udtHashPasswordModel.HashedValue), _
                                db.MakeInParam("@User_password_level", SqlDbType.Int, 4, udtHashPasswordModel.PasswordLevel), _
                                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, .UserACTSMP) _
                            }
                            db.RunProc("proc_HCVUUserAC_upd_Password_Transit", parms)
                            db.CommitTransaction()
                            blnResult = True

                        Case EnumPlatformType.IVRS

                        Case EnumPlatformType.ICW
                            Dim prams() As SqlParameter = { _
                                db.MakeInParam("@Staff_ID", SqlDbType.Char, 8, .ID1), _
                                db.MakeInParam("@Staff_Password", SqlDbType.VarChar, 255, udtHashPasswordModel.HashedValue), _
                                db.MakeInParam("@Staff_Password_Level", SqlDbType.Int, 4, udtHashPasswordModel.PasswordLevel)
                            }
                            db.RunProc("proc_ICWStaffAccount_Update_ByStaffID", prams)
                            db.CommitTransaction()
                            blnResult = True

                    End Select
                Catch ex As Exception
                    db.RollBackTranscation()
                    Throw ex
                Finally
                    If Not db Is Nothing Then db.Dispose()
                End Try

            End With

            Return blnResult
        End Function

        Private Shared Function GetColumnValue(ByVal dt As DataTable, ByVal arrColName As String(), ByVal type As Type) As Object
            Dim objOutput As Object
            Dim bln As Boolean = False

            Select Case type
                Case GetType(String)
                    objOutput = String.Empty
                Case GetType(Byte())
                    objOutput = Nothing
                Case Else
                    objOutput = Nothing
            End Select

            For Each str As String In arrColName
                If Not bln Then
                    If dt.Columns.Contains(str) AndAlso Not dt.Rows(0).Item(str) Is DBNull.Value Then
                        objOutput = dt.Rows(0).Item(str)
                        bln = True
                    End If
                Else
                    Exit For
                End If
            Next

            Return objOutput

        End Function

        Private Shared Function GetOriginalIVRS(ByVal strUserID As String) As DataTable
            Dim dtUser As New DataTable
            Dim db As New Database

            Try
                db.BeginTransaction()
                Dim parms() As SqlParameter = { _
                            db.MakeInParam("@SP_ID", SqlDbType.Char, 20, strUserID)}
                db.RunProc("proc_IVRSPasswordOriginal_get", parms, dtUser)

                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
            Return dtUser
        End Function

        Private Shared Sub UpdateOriginalIVRSStatus(ByVal strUserID As String)
            Dim db As New Database
            Try
                Dim parms() As SqlParameter = { _
                            db.MakeInParam("@SP_ID", SqlDbType.Char, 20, strUserID)}
                db.RunProc("proc_IVRSPasswordOriginal_upd", parms)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub
#End Region


#Region "Encrypt Function"

        Private Shared Function SHA512Hash(ByVal strSourceText As String) As String
            Dim objUnicodeEncoding As New UnicodeEncoding
            Dim bytSourceText() As Byte = objUnicodeEncoding.GetBytes(strSourceText)
            Dim sha512 As New SHA512CryptoServiceProvider
            Dim bytHash() As Byte = sha512.ComputeHash(bytSourceText)
            Return Convert.ToBase64String(bytHash)
        End Function
#End Region


#Region "Model"
        Public Class HashModel
            Private _intPasswordLevel As Integer
            Private _strHashedValue As String

            ''' <summary>
            '''  default value: String empty
            ''' </summary>
            Public Property HashedValue() As String
                Get
                    Return _strHashedValue
                End Get
                Set(ByVal value As String)
                    _strHashedValue = value
                End Set
            End Property

            ''' <summary>
            '''  default value: nothing
            ''' </summary>
            Public Property PasswordLevel() As String
                Get
                    Return _intPasswordLevel
                End Get
                Set(ByVal value As String)
                    _intPasswordLevel = value
                End Set
            End Property

            Public Sub New()
                _strHashedValue = String.Empty
                _intPasswordLevel = Nothing
            End Sub

            Public Sub New(ByVal strHashedValue As String, ByVal intPasswordLevel As Integer)
                Me.New()
                _strHashedValue = strHashedValue
                _intPasswordLevel = intPasswordLevel
            End Sub

        End Class

        Public Class TransitPasswordModel
            Private _strID1 As String
            Private _strID2 As String
            Private _strInputPassword As String
            Protected _UserACTSMP As Byte()

            Public Property ID1() As String
                Get
                    Return _strID1
                End Get
                Set(ByVal value As String)
                    _strID1 = value
                End Set
            End Property

            Public Property ID2() As String
                Get
                    Return _strID2
                End Get
                Set(ByVal value As String)
                    _strID2 = value
                End Set
            End Property

            Public Property InputPassword() As String
                Get
                    Return _strInputPassword
                End Get
                Set(ByVal value As String)
                    _strInputPassword = value
                End Set
            End Property

            Public Property UserACTSMP() As Byte()
                Get
                    Return _UserACTSMP
                End Get
                Set(ByVal value As Byte())
                    _UserACTSMP = value
                End Set
            End Property

            Public Sub New(ByVal strID1 As String, ByVal strID2 As String, ByVal strInputPassword As String, ByVal strUserACTSMP As Byte())
                _strID1 = strID1
                _strID2 = strID2
                _strInputPassword = strInputPassword
                _UserACTSMP = strUserACTSMP
            End Sub

        End Class

        Public Class VerifyPasswordResultModel
            Private _enumVerifyResult As EnumVerifyPasswordResult
            Private _blnTransitPassword As Boolean

            Public Property VerifyResult() As EnumVerifyPasswordResult
                Get
                    Return _enumVerifyResult
                End Get
                Set(ByVal value As EnumVerifyPasswordResult)
                    _enumVerifyResult = value
                End Set
            End Property

            Public Property TransitPassword() As Boolean
                Get
                    Return _blnTransitPassword
                End Get
                Set(ByVal value As Boolean)
                    _blnTransitPassword = value
                End Set
            End Property

            Public Sub New()
                _enumVerifyResult = EnumVerifyPasswordResult.Incorrect
                _blnTransitPassword = False
            End Sub

            Public Sub New(ByVal enumVerifyResult As EnumVerifyPasswordResult, ByVal blnTransitPassword As Boolean)
                Me.New()
                _enumVerifyResult = enumVerifyResult
                _blnTransitPassword = blnTransitPassword
            End Sub

        End Class
#End Region
    End Class

    
End Namespace