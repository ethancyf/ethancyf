Imports System.Data
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component
Imports Common.ComObject


'-------------------------------------------------------------------------------------------
'-------------------------------------------------------------------------------------------
'                                    Handle System Error 
'-------------------------------------------------------------------------------------------
'--------------------------------------------------------------------------------------------

Namespace ComObject

    Public Class ErrorLogHandler

#Region "Constructors"

        Private Shared _ErrorHandler As ErrorLogHandler

        Public Shared Function getInstance(ByVal auditFunc As String) As ErrorLogHandler
            If _ErrorHandler Is Nothing Then

                Select Case auditFunc
                    Case EnumAuditLog.UploadClaim
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070101)
                    Case EnumAuditLog.GetReasonForVisitList
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070102)
                    Case EnumAuditLog.RCHNameQuery
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070103)
                    Case EnumAuditLog.eHSValidatedAccountQuery
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070104)
                    Case EnumAuditLog.eHSAccountSubsidyQuery
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070105)
                    Case EnumAuditLog.SPPracticeValidation
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070106)
                    Case Else
                        _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070101)
                End Select
            End If
            Return _ErrorHandler
        End Function

        Public Shared Function getInstance() As ErrorLogHandler
            If _ErrorHandler Is Nothing Then
                _ErrorHandler = New ErrorLogHandler(Common.Component.FunctCode.FUNT070101)
            End If
            Return _ErrorHandler
        End Function


        Private Sub New(ByVal strFunctionCode As String)
            Me._strFunctionCode = strFunctionCode
        End Sub

        Private Sub New()
        End Sub

#End Region

        Private _strDBFlag As String = Common.Component.DBFlagStr.DBFlag2
        Private _strFunctionCode As String = ""

        ''' <summary>
        ''' Create Database by different DBFlag
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateDatabase() As Database
            Dim db As Database
            If Me._strDBFlag = String.Empty Then
                db = New Database
            Else
                db = New Database(Me._strDBFlag)
            End If

            Return db
        End Function

        Public Sub WriteSystemLogToDB(ByVal strDescription As String, Optional ByVal strUserID As String = "")

            Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
            Dim strClientIP As String
            Dim strPhysicalPath As String = String.Empty

            'Client IP & Session ID
            Try
                strClientIP = HttpContext.Current.Request.UserHostAddress
            Catch ex As Exception
                strClientIP = String.Empty
            End Try

            Try
                strPhysicalPath = HttpContext.Current.Request.PhysicalPath
            Catch ex As Exception
                strPhysicalPath = String.Empty
            End Try

            ' Me.AddSystemInterfaceLog(strClientIP, strUserID, strSessionID, strBrowser, strOS, strDescription)

            Dim objDatabase As Database = CreateDatabase()
            Dim strSeverityCode As String = String.Empty
            strSeverityCode = Chr(ErrorHandler.EnumSeverityCode.Unknown)

            ErrorHandler.Log(objDatabase, _strFunctionCode, strSeverityCode, "99999", strPhysicalPath, strClientIP, strUserID, strDescription)

        End Sub

    End Class

End Namespace


