Imports System.IO
Imports System.Diagnostics
Imports Common.Component
Imports CommonScheduleJob.Logger
Imports System.Security.Cryptography
Imports System.Text
Imports System.Configuration
Imports Common.ComObject
Imports Common.DataAccess
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports System.Runtime.InteropServices
Imports System.Reflection

Public Class ImmdTransfer
    Inherits CommonScheduleJob.BaseScheduleJob

    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpdefault As String, ByVal lpretrunedstring As String, ByVal nSize As Int32, ByVal lpFilename As String) As Int32

    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return String.Empty 'Common.Component.ScheduleJobID.ImmdTransfer
        End Get
    End Property

    Protected Overrides Sub Process()

        Dim ImmdSetting As Setting = New Setting

        Try

            ImmdSetting = readSetting(ImmdSetting)

            replaceClassParameters(ImmdSetting)

            insertLog(ImmdSetting, "====================================================================================================")

            logSetting(ImmdSetting)

            transfer(ImmdSetting)

            insertLog(ImmdSetting, "====================================================================================================")

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

    End Sub

    Public Class Setting

        Protected strAction As String = String.Empty
        Protected strParaFilePath As String = String.Empty
        Protected strSection As String = String.Empty
        Protected strExecutionDate As String = String.Empty
        Protected strLogPath As String = String.Empty

        Protected strEHSWebServer As String = String.Empty
        Protected strRootFolder As String = String.Empty
        Protected strTemplateFolder As String = String.Empty
        Protected strSectionFolder As String = String.Empty
        Protected strScriptFolder As String = String.Empty
        Protected strFileStore As String = String.Empty
        Protected strContentFileSuffix As String = String.Empty
        Protected strControlFileSuffix As String = String.Empty

        Protected strGet_last_success_step1_template_file As String = String.Empty
        Protected strGet_last_success_step1_generated_file As String = String.Empty
        Protected strGet_last_success_step2_template_file As String = String.Empty
        Protected strGet_last_success_step2_generated_file As String = String.Empty
        Protected strPut_last_success_step1_template_file As String = String.Empty
        Protected strPut_last_success_step1_generated_file As String = String.Empty
        Protected strPut_last_success_step2_template_file As String = String.Empty
        Protected strPut_last_success_step2_generated_file As String = String.Empty

        Protected strStep1_template_file As String = String.Empty
        Protected strStep1_generated_file As String = String.Empty
        Protected strStep2_template_file As String = String.Empty
        Protected strStep2_generated_file As String = String.Empty
        Protected strStep3_template_file As String = String.Empty
        Protected strStep3_generated_file As String = String.Empty
        Protected strStep4_template_file As String = String.Empty
        Protected strStep4_generated_file As String = String.Empty
        Protected strStep5_template_file As String = String.Empty
        Protected strStep5_generated_file As String = String.Empty
        Protected strStep6_template_file As String = String.Empty
        Protected strStep6_generated_file As String = String.Empty

        Protected strFilePrefix As String = String.Empty
        Protected strLastSuccessIndicator As String = String.Empty
        Protected strLastSuccessIndicatorSyn As String = String.Empty
        Protected strImmDFolder As String = String.Empty

        Protected strControlFileName As String = String.Empty
        Protected strcontentFileName As String = String.Empty

        Public Const CONST_eHSWebServer As String = "eHSWebServer"
        Public Const CONST_rootFolder As String = "rootFolder"
        Public Const CONST_templateFolder As String = "templateFolder"
        Public Const CONST_sectionFolder As String = "sectionFolder"
        Public Const CONST_scriptFolder As String = "scriptFolder"

        Public Const CONST_fileStore As String = "fileStore"
        Public Const CONST_contentFileSuffix As String = "contentFileSuffix"
        Public Const CONST_controlFileSuffix As String = "controlFileSuffix"

        Public Const CONST_get_last_success_step1_template_file As String = "get_last_success_step1_template_file"
        Public Const CONST_get_last_success_step1_generated_file As String = "get_last_success_step1_generated_file"
        Public Const CONST_get_last_success_step2_template_file As String = "get_last_success_step2_template_file"
        Public Const CONST_get_last_success_step2_generated_file As String = "get_last_success_step2_generated_file"
        Public Const CONST_put_last_success_step1_template_file As String = "put_last_success_step1_template_file"
        Public Const CONST_put_last_success_step1_generated_file As String = "put_last_success_step1_generated_file"
        Public Const CONST_put_last_success_step2_template_file As String = "put_last_success_step2_template_file"
        Public Const CONST_put_last_success_step2_generated_file As String = "put_last_success_step2_generated_file"

        Public Const CONST_step1_template_file As String = "step1_template_file"
        Public Const CONST_step1_generated_file As String = "step1_generated_file"
        Public Const CONST_step2_template_file As String = "step2_template_file"
        Public Const CONST_step2_generated_file As String = "step2_generated_file"
        Public Const CONST_step3_template_file As String = "step3_template_file"
        Public Const CONST_step3_generated_file As String = "step3_generated_file"

        Public Const CONST_step4_template_file As String = "step4_template_file"
        Public Const CONST_step4_generated_file As String = "step4_generated_file"
        Public Const CONST_step5_template_file As String = "step5_template_file"
        Public Const CONST_step5_generated_file As String = "step5_generated_file"
        Public Const CONST_step6_template_file As String = "step6_template_file"
        Public Const CONST_step6_generated_file As String = "step6_generated_file"

        Public Const CONST_filePrefix As String = "filePrefix"
        Public Const CONST_lastSuccessIndicator As String = "lastSuccessIndicator"
        Public Const CONST_lastSuccessIndicatorSyn As String = "lastSuccessIndicatorSyn"
        Public Const CONST_ImmDFolder As String = "ImmDFolder"

        Public Const CONST_contentFileName As String = "contentFileName"
        Public Const CONST_controlFileName As String = "controlFileName"
        Public Const CONST_executionDate As String = "executionDate"

        Public Sub New()
        End Sub

        Public Property action() As String
            Get
                Return Me.strAction
            End Get
            Set(ByVal value As String)
                Me.strAction = value
            End Set
        End Property

        Public Property paraFilePath() As String
            Get
                Return Me.strParaFilePath
            End Get
            Set(ByVal value As String)
                Me.strParaFilePath = value
            End Set
        End Property

        Public Property section() As String
            Get
                Return Me.strSection
            End Get
            Set(ByVal value As String)
                Me.strSection = value
            End Set
        End Property

        Public Property executionDate() As String
            Get
                Return Me.strExecutionDate
            End Get
            Set(ByVal value As String)
                Me.strExecutionDate = value
            End Set
        End Property

        Public Property logPath() As String
            Get
                Return Me.strLogPath
            End Get
            Set(ByVal value As String)
                Me.strLogPath = value
            End Set
        End Property



        Public Property eHSWebServer() As String
            Get
                Return Me.strEHSWebServer
            End Get
            Set(ByVal value As String)
                Me.strEHSWebServer = value
            End Set
        End Property

        Public Property rootFolder() As String
            Get
                Return Me.strRootFolder
            End Get
            Set(ByVal value As String)
                Me.strRootFolder = value
            End Set
        End Property

        Public Property templateFolder() As String
            Get
                Return Me.strTemplateFolder
            End Get
            Set(ByVal value As String)
                Me.strTemplateFolder = value
            End Set
        End Property

        Public Property sectionFolder() As String
            Get
                Return Me.strSectionFolder
            End Get
            Set(ByVal value As String)
                Me.strSectionFolder = value
            End Set
        End Property

        Public Property scriptFolder() As String
            Get
                Return Me.strScriptFolder
            End Get
            Set(ByVal value As String)
                Me.strScriptFolder = value
            End Set
        End Property

        Public Property fileStore() As String
            Get
                Return Me.strFileStore
            End Get
            Set(ByVal value As String)
                Me.strFileStore = value
            End Set
        End Property

        Public Property contentFileSuffix() As String
            Get
                Return Me.strContentFileSuffix
            End Get
            Set(ByVal value As String)
                Me.strContentFileSuffix = value
            End Set
        End Property

        Public Property controlFileSuffix() As String
            Get
                Return Me.strControlFileSuffix
            End Get
            Set(ByVal value As String)
                Me.strControlFileSuffix = value
            End Set
        End Property



        Public Property get_last_success_step1_template_file() As String
            Get
                Return Me.strGet_last_success_step1_template_file
            End Get
            Set(ByVal value As String)
                Me.strGet_last_success_step1_template_file = value
            End Set
        End Property

        Public Property get_last_success_step1_generated_file() As String
            Get
                Return Me.strGet_last_success_step1_generated_file
            End Get
            Set(ByVal value As String)
                Me.strGet_last_success_step1_generated_file = value
            End Set
        End Property

        Public Property get_last_success_step2_template_file() As String
            Get
                Return Me.strGet_last_success_step2_template_file
            End Get
            Set(ByVal value As String)
                Me.strGet_last_success_step2_template_file = value
            End Set
        End Property

        Public Property get_last_success_step2_generated_file() As String
            Get
                Return Me.strGet_last_success_step2_generated_file
            End Get
            Set(ByVal value As String)
                Me.strGet_last_success_step2_generated_file = value
            End Set
        End Property

        Public Property put_last_success_step1_template_file() As String
            Get
                Return Me.strPut_last_success_step1_template_file
            End Get
            Set(ByVal value As String)
                Me.strPut_last_success_step1_template_file = value
            End Set
        End Property

        Public Property put_last_success_step1_generated_file() As String
            Get
                Return Me.strPut_last_success_step1_generated_file
            End Get
            Set(ByVal value As String)
                Me.strPut_last_success_step1_generated_file = value
            End Set
        End Property

        Public Property put_last_success_step2_template_file() As String
            Get
                Return Me.strPut_last_success_step2_template_file
            End Get
            Set(ByVal value As String)
                Me.strPut_last_success_step2_template_file = value
            End Set
        End Property

        Public Property put_last_success_step2_generated_file() As String
            Get
                Return Me.strPut_last_success_step2_generated_file
            End Get
            Set(ByVal value As String)
                Me.strPut_last_success_step2_generated_file = value
            End Set
        End Property



        Public Property step1_template_file() As String
            Get
                Return Me.strStep1_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep1_template_file = value
            End Set
        End Property

        Public Property step1_generated_file() As String
            Get
                Return Me.strStep1_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep1_generated_file = value
            End Set
        End Property

        Public Property step2_template_file() As String
            Get
                Return Me.strStep2_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep2_template_file = value
            End Set
        End Property

        Public Property step2_generated_file() As String
            Get
                Return Me.strStep2_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep2_generated_file = value
            End Set
        End Property

        Public Property step3_template_file() As String
            Get
                Return Me.strStep3_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep3_template_file = value
            End Set
        End Property

        Public Property step3_generated_file() As String
            Get
                Return Me.strStep3_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep3_generated_file = value
            End Set
        End Property

        Public Property step4_template_file() As String
            Get
                Return Me.strStep4_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep4_template_file = value
            End Set
        End Property

        Public Property step4_generated_file() As String
            Get
                Return Me.strStep4_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep4_generated_file = value
            End Set
        End Property

        Public Property step5_template_file() As String
            Get
                Return Me.strStep5_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep5_template_file = value
            End Set
        End Property

        Public Property step5_generated_file() As String
            Get
                Return Me.strStep5_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep5_generated_file = value
            End Set
        End Property

        Public Property step6_template_file() As String
            Get
                Return Me.strStep6_template_file
            End Get
            Set(ByVal value As String)
                Me.strStep6_template_file = value
            End Set
        End Property

        Public Property step6_generated_file() As String
            Get
                Return Me.strStep6_generated_file
            End Get
            Set(ByVal value As String)
                Me.strStep6_generated_file = value
            End Set
        End Property



        Public Property filePrefix() As String
            Get
                Return Me.strFilePrefix
            End Get
            Set(ByVal value As String)
                Me.strFilePrefix = value
            End Set
        End Property

        Public Property lastSuccessIndicator() As String
            Get
                Return Me.strLastSuccessIndicator
            End Get
            Set(ByVal value As String)
                Me.strLastSuccessIndicator = value
            End Set
        End Property

        Public Property lastSuccessIndicatorSyn() As String
            Get
                Return Me.strLastSuccessIndicatorSyn
            End Get
            Set(ByVal value As String)
                Me.strLastSuccessIndicatorSyn = value
            End Set
        End Property

        Public Property ImmDFolder() As String
            Get
                Return Me.strImmDFolder
            End Get
            Set(ByVal value As String)
                Me.strImmDFolder = value
            End Set
        End Property



        Public Property controlFileName() As String
            Get
                Return Me.strControlFileName
            End Get
            Set(ByVal value As String)
                Me.strControlFileName = value
            End Set
        End Property

        Public Property contentFileName() As String
            Get
                Return Me.strcontentFileName
            End Get
            Set(ByVal value As String)
                Me.strcontentFileName = value
            End Set
        End Property

    End Class

    Function readSetting(ByRef ImmdSetting As Setting) As Setting

        Dim strCommonSection As String = "Common"

        'Read From Command Line Arguments
        ImmdSetting.action = Me.Args(0).ToString
        ImmdSetting.paraFilePath = Me.Args(1).ToString
        ImmdSetting.section = Me.Args(3).ToString
        If UBound(Me.Args) = 3 Then
            ImmdSetting.executionDate = getMatchDateString(Now)
        ElseIf UBound(Me.Args) = 4 Then
            ImmdSetting.executionDate = Me.Args(4).ToString.Trim
        End If
        ImmdSetting.logPath = Me.Args(2).ToString + "\ImmdTransfer_" + ImmdSetting.executionDate + ".log"

        'Read From Setting File [Common]
        ImmdSetting.eHSWebServer = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_eHSWebServer, String.Empty)
        ImmdSetting.rootFolder = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_rootFolder, String.Empty)
        ImmdSetting.templateFolder = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_templateFolder, String.Empty)
        ImmdSetting.sectionFolder = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_sectionFolder, String.Empty)
        ImmdSetting.scriptFolder = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_scriptFolder, String.Empty)
        ImmdSetting.fileStore = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_fileStore, String.Empty)
        ImmdSetting.contentFileSuffix = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_contentFileSuffix, String.Empty)
        ImmdSetting.controlFileSuffix = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_controlFileSuffix, String.Empty)

        ImmdSetting.get_last_success_step1_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_get_last_success_step1_template_file, String.Empty)
        ImmdSetting.get_last_success_step1_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_get_last_success_step1_generated_file, String.Empty)
        ImmdSetting.get_last_success_step2_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_get_last_success_step2_template_file, String.Empty)
        ImmdSetting.get_last_success_step2_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_get_last_success_step2_generated_file, String.Empty)
        ImmdSetting.put_last_success_step1_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_put_last_success_step1_template_file, String.Empty)
        ImmdSetting.put_last_success_step1_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_put_last_success_step1_generated_file, String.Empty)
        ImmdSetting.put_last_success_step2_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_put_last_success_step2_template_file, String.Empty)
        ImmdSetting.put_last_success_step2_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_put_last_success_step2_generated_file, String.Empty)

        ImmdSetting.step1_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step1_template_file, String.Empty)
        ImmdSetting.step1_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step1_generated_file, String.Empty)
        ImmdSetting.step2_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step2_template_file, String.Empty)
        ImmdSetting.step2_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step2_generated_file, String.Empty)
        ImmdSetting.step3_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step3_template_file, String.Empty)
        ImmdSetting.step3_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step3_generated_file, String.Empty)
        ImmdSetting.step4_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step4_template_file, String.Empty)
        ImmdSetting.step4_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step4_generated_file, String.Empty)
        ImmdSetting.step5_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step5_template_file, String.Empty)
        ImmdSetting.step5_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step5_generated_file, String.Empty)
        ImmdSetting.step6_template_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step6_template_file, String.Empty)
        ImmdSetting.step6_generated_file = getIniValue(ImmdSetting, strCommonSection, Setting.CONST_step6_generated_file, String.Empty)

        'Read From Setting File [Section]
        ImmdSetting.filePrefix = getIniValue(ImmdSetting, ImmdSetting.section, Setting.CONST_filePrefix, String.Empty) + ImmdSetting.executionDate
        ImmdSetting.lastSuccessIndicator = getIniValue(ImmdSetting, ImmdSetting.section, Setting.CONST_lastSuccessIndicator, String.Empty)
        ImmdSetting.lastSuccessIndicatorSyn = getIniValue(ImmdSetting, ImmdSetting.section, Setting.CONST_lastSuccessIndicatorSyn, String.Empty)
        ImmdSetting.ImmDFolder = getIniValue(ImmdSetting, ImmdSetting.section, Setting.CONST_ImmDFolder, String.Empty)

        Return ImmdSetting

    End Function

    Sub logSetting(ByVal ImmdSetting As Setting)

        insertLog(ImmdSetting, "action = " + ImmdSetting.action)
        insertLog(ImmdSetting, "section = " + ImmdSetting.section)
        insertLog(ImmdSetting, "paraFilePath = " + ImmdSetting.paraFilePath)
        insertLog(ImmdSetting, "executionDate = " + ImmdSetting.executionDate)
        insertLog(ImmdSetting, "logPath = " + ImmdSetting.logPath)

        insertLog(ImmdSetting, Setting.CONST_eHSWebServer + " = " + ImmdSetting.eHSWebServer)
        insertLog(ImmdSetting, Setting.CONST_rootFolder + " = " + ImmdSetting.rootFolder)
        insertLog(ImmdSetting, Setting.CONST_templateFolder + " = " + ImmdSetting.templateFolder)
        insertLog(ImmdSetting, Setting.CONST_sectionFolder + " = " + ImmdSetting.sectionFolder)
        insertLog(ImmdSetting, Setting.CONST_scriptFolder + " = " + ImmdSetting.scriptFolder)
        insertLog(ImmdSetting, Setting.CONST_contentFileSuffix + " = " + ImmdSetting.contentFileSuffix)
        insertLog(ImmdSetting, Setting.CONST_controlFileSuffix + " = " + ImmdSetting.controlFileSuffix)
        insertLog(ImmdSetting, Setting.CONST_fileStore + " = " + ImmdSetting.fileStore)

        insertLog(ImmdSetting, Setting.CONST_get_last_success_step1_template_file + " = " + ImmdSetting.get_last_success_step1_template_file)
        insertLog(ImmdSetting, Setting.CONST_get_last_success_step1_generated_file + " = " + ImmdSetting.get_last_success_step1_generated_file)
        insertLog(ImmdSetting, Setting.CONST_get_last_success_step2_template_file + " = " + ImmdSetting.get_last_success_step2_template_file)
        insertLog(ImmdSetting, Setting.CONST_get_last_success_step2_generated_file + " = " + ImmdSetting.get_last_success_step2_generated_file)
        insertLog(ImmdSetting, Setting.CONST_put_last_success_step1_template_file + " = " + ImmdSetting.put_last_success_step1_template_file)
        insertLog(ImmdSetting, Setting.CONST_put_last_success_step1_generated_file + " = " + ImmdSetting.put_last_success_step1_generated_file)
        insertLog(ImmdSetting, Setting.CONST_get_last_success_step2_template_file + " = " + ImmdSetting.get_last_success_step2_template_file)
        insertLog(ImmdSetting, Setting.CONST_get_last_success_step2_generated_file + " = " + ImmdSetting.get_last_success_step2_generated_file)

        insertLog(ImmdSetting, Setting.CONST_step1_template_file + " = " + ImmdSetting.step1_template_file)
        insertLog(ImmdSetting, Setting.CONST_step1_generated_file + " = " + ImmdSetting.step1_generated_file)
        insertLog(ImmdSetting, Setting.CONST_step2_template_file + " = " + ImmdSetting.step2_template_file)
        insertLog(ImmdSetting, Setting.CONST_step2_generated_file + " = " + ImmdSetting.step2_generated_file)
        insertLog(ImmdSetting, Setting.CONST_step3_template_file + " = " + ImmdSetting.step3_template_file)
        insertLog(ImmdSetting, Setting.CONST_step3_generated_file + " = " + ImmdSetting.step3_generated_file)
        insertLog(ImmdSetting, Setting.CONST_step4_template_file + " = " + ImmdSetting.step4_template_file)
        insertLog(ImmdSetting, Setting.CONST_step4_generated_file + " = " + ImmdSetting.step4_generated_file)
        insertLog(ImmdSetting, Setting.CONST_step5_template_file + " = " + ImmdSetting.step5_template_file)
        insertLog(ImmdSetting, Setting.CONST_step5_generated_file + " = " + ImmdSetting.step5_generated_file)
        insertLog(ImmdSetting, Setting.CONST_step6_template_file + " = " + ImmdSetting.step6_template_file)
        insertLog(ImmdSetting, Setting.CONST_step6_generated_file + " = " + ImmdSetting.step6_generated_file)

        insertLog(ImmdSetting, Setting.CONST_filePrefix + " = " + ImmdSetting.filePrefix)
        insertLog(ImmdSetting, Setting.CONST_lastSuccessIndicator + " = " + ImmdSetting.lastSuccessIndicator)
        insertLog(ImmdSetting, Setting.CONST_lastSuccessIndicatorSyn + " = " + ImmdSetting.lastSuccessIndicatorSyn)
        insertLog(ImmdSetting, Setting.CONST_ImmDFolder + " = " + ImmdSetting.ImmDFolder)

    End Sub

    Function getIniValue(ByVal ImmdSetting As Setting, ByVal strSection As String, ByVal strName As String, ByVal strDefaultValue As String) As String
        Dim strGetValue As String = String.Empty

        Try

            Dim strIni As String
            Dim strValueLength As Integer = 255
            strIni = New String(" ", strValueLength)
            Dim bufferLength As Long = 0

            bufferLength = GetPrivateProfileString(strSection, strName, strDefaultValue, strIni, strValueLength, ImmdSetting.paraFilePath)

            If bufferLength = 0 Then
                strGetValue = strDefaultValue.ToString
            Else
                strGetValue = Left(strIni, bufferLength).ToString
            End If

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return strGetValue
    End Function

    Function readIniFile(ByVal ImmdSetting As Setting, ByVal strFilePath As String, ByVal strSection As String) As String

        Dim strGetValue As String = String.Empty

        Try
            Dim objINIRead As New StreamReader(strFilePath)
            Dim strTheINI As String = objINIRead.ReadToEnd
            Dim i As Integer
            Dim intLine As Integer
            Dim blnNoSection As Boolean = False
            Dim strLineStream As Object

            strLineStream = strTheINI.Split(Chr(13))
            intLine = UBound(strLineStream)

            For i = 0 To intLine
                If strLineStream(i).indexof("=") > 0 Then
                    If strLineStream(i).split("=")(0).trim() = strSection Then
                        blnNoSection = True
                        strGetValue = strLineStream(i).split("=")(1).trim()
                        Exit For
                    End If
                End If
            Next i
        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return strGetValue

    End Function

    'Function GenerateSFTPAccountFile(ByVal ImmdSetting As Setting) As Boolean

    '    Dim BeSuccess As Boolean = False

    '    Try

    '        ClearSFTPAccountFile(ImmdSetting)

    '        Dim strKeyFile As String = "C:\EVS\key.txt"    'Hard code the key file on purpose
    '        'insertLog(ImmdSetting, "Generate SFTP account file " + ImmdSetting.encryptedFilePath + " from " + ImmdSetting.decryptedFilePath)
    '        'CryptFile(getFileContents(ImmdSetting, strKeyFile), ImmdSetting.decryptedFilePath, ImmdSetting.encryptedFilePath, True)
    '        'CryptFile(getFileContents(ImmdSetting, strKeyFile), ImmdSetting.encryptedFilePath, ImmdSetting.decryptedFilePath, False)
    '        BeSuccess = True

    '    Catch ex As Exception

    '        insertErrorLog(ImmdSetting, ex.ToString)

    '    End Try

    '    Return BeSuccess
    'End Function

    'Sub ClearSFTPAccountFile(ByVal ImmdSetting As Setting)
    '    Try
    '        insertLog(ImmdSetting, "Delete SFTP account file " + ImmdSetting.decryptedFilePath)
    '        Me.DeleteFile(ImmdSetting, ImmdSetting.decryptedFilePath)
    '    Catch ex As Exception

    '        insertErrorLog(ImmdSetting, ex.ToString)

    '    End Try
    'End Sub

    Sub transfer(ByVal ImmdSetting As Setting)

        Try

            Dim BeContinue As Boolean = True

            insertLog(ImmdSetting, "Start Process")

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.get_last_success_step1_generated_file, ImmdSetting.get_last_success_step1_template_file)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.get_last_success_step2_generated_file, ImmdSetting.get_last_success_step2_template_file)
            End If

            If BeContinue Then
                BeContinue = execute(ImmdSetting, ImmdSetting.get_last_success_step2_generated_file)
            End If

            If BeContinue Then
                BeContinue = shouldExecuteToday(ImmdSetting)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step1_generated_file, ImmdSetting.step1_template_file)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step2_generated_file, ImmdSetting.step2_template_file)
            End If

            If BeContinue Then
                BeContinue = execute(ImmdSetting, ImmdSetting.step2_generated_file)
                'Debug : bypass SFTP error
                'BeContinue = True
            End If

            If BeContinue Then
                BeContinue = IdentityDocumentSetFound(ImmdSetting)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step3_generated_file, ImmdSetting.step3_template_file)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step4_generated_file, ImmdSetting.step4_template_file)
            End If

            If BeContinue Then
                BeContinue = execute(ImmdSetting, ImmdSetting.step4_generated_file)
                'Debug : bypass SFTP error
                'BeContinue = True
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step5_generated_file, ImmdSetting.step5_template_file)
            End If

            If BeContinue Then
                BeContinue = generateFileFromTemplateFile(ImmdSetting, ImmdSetting.step6_generated_file, ImmdSetting.step6_template_file)
            End If

            If BeContinue Then
                BeContinue = execute(ImmdSetting, ImmdSetting.step6_generated_file)
                'Debug : bypass SFTP error
                'BeContinue = True
            End If

            If BeContinue Then
                writeToFile(ImmdSetting, getFormattedDateTime(ImmdSetting, Now), ImmdSetting.lastSuccessIndicator, False)
                UploadLastSuccessIndicator(ImmdSetting)
            End If

            insertLog(ImmdSetting, "End Process")

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

    End Sub

    Sub UploadLastSuccessIndicator(ByVal ImmdSetting As Setting)
        generateFileFromTemplateFile(ImmdSetting, ImmdSetting.put_last_success_step1_generated_file, ImmdSetting.put_last_success_step1_template_file)
        generateFileFromTemplateFile(ImmdSetting, ImmdSetting.put_last_success_step2_generated_file, ImmdSetting.put_last_success_step2_template_file)
        execute(ImmdSetting, ImmdSetting.put_last_success_step2_generated_file)
    End Sub

    Function IdentityDocumentSetFound(ByVal ImmdSetting As Setting) As Boolean

        Dim beSuccess As Boolean = True

        Dim DirectoryInfo As New IO.DirectoryInfo(ImmdSetting.fileStore)
        Dim FileInfoList As IO.FileInfo() = DirectoryInfo.GetFiles("*" + ImmdSetting.executionDate() + "*")
        Dim FileInfo As IO.FileInfo

        insertLog(ImmdSetting, "File List Length: " + FileInfoList.Length.ToString)

        If FileInfoList.Length = 2 Then
            beSuccess = False
        End If

        Dim blnControlFileFound As Boolean = False
        Dim blnContentFileFound As Boolean = False

        For Each FileInfo In FileInfoList
            insertLog(ImmdSetting, "File name = " + FileInfo.Name)
            If Not blnContentFileFound And matchFilePrefixSuffix(ImmdSetting, FileInfo.Name, ImmdSetting.filePrefix, ImmdSetting.contentFileSuffix) Then
                ImmdSetting.contentFileName = FileInfo.Name
                insertLog(ImmdSetting, "Matched content file found: " + ImmdSetting.contentFileName)
                blnContentFileFound = True
            End If
            If Not blnControlFileFound And matchFilePrefixSuffix(ImmdSetting, FileInfo.Name, ImmdSetting.filePrefix, ImmdSetting.controlFileSuffix) Then
                ImmdSetting.controlFileName = FileInfo.Name
                insertLog(ImmdSetting, "Matched control file found: " + ImmdSetting.controlFileName)
                blnControlFileFound = True
            End If
        Next

        If Not blnControlFileFound Then
            insertLog(ImmdSetting, "No matched control file found!")
        End If

        If Not blnContentFileFound Then
            insertLog(ImmdSetting, "No matched content file found!")
        End If

        beSuccess = blnControlFileFound And blnContentFileFound

        Return beSuccess

    End Function

    Function matchFilePrefixSuffix(ByVal ImmdSetting As Setting, ByVal strFileName As String, ByVal strFileNamePrefix As String, ByVal strFileNameSuffix As String) As Boolean

        Dim match As Boolean = False

        Try

            If strFileName.ToLower.StartsWith(strFileNamePrefix.ToLower) And strFileName.ToLower.EndsWith(strFileNameSuffix.ToLower) Then
                match = True
            End If

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return match

    End Function

    Sub insertErrorLog(ByVal ImmdSetting As Setting, ByVal strMessage As String)

        System.Console.WriteLine("Exception Error:")
        System.Console.WriteLine(strMessage)
        System.Console.WriteLine("For details, please find the detailed log : {0}", ImmdSetting.logPath)
        insertLog(ImmdSetting, strMessage)

    End Sub

    Sub insertLog(ByVal ImmdSetting As Setting, ByVal strMessage As String)

        writeToFile(ImmdSetting, "<" + getFormattedDateTime(ImmdSetting, Now) + ">" + " : " + strMessage, ImmdSetting.logPath, True)

    End Sub

    Function shouldExecuteToday(ByVal ImmdSetting As Setting) As Boolean

        Dim blnShouldRunMainFlow As Boolean = False

        Dim strSendToImmdControlFileSuffix As String = ".cf"
        Dim strReceiveFromImmdControlFileSuffix As String = ".rcf"

        Dim strSendToImmdAction As String = "send"
        Dim strReceiveFromImmdAction As String = "receive"

        Dim SynIsToday As Boolean = checkDateIsToday(ImmdSetting, ImmdSetting.lastSuccessIndicatorSyn)
        Dim LocalIsToday As Boolean = checkDateIsToday(ImmdSetting, ImmdSetting.lastSuccessIndicator)

        Dim RunProgram As Boolean = True
        Try
            If (ImmdSetting.action.ToLower = strSendToImmdAction And ImmdSetting.controlFileSuffix.ToLower = strSendToImmdControlFileSuffix) Or (ImmdSetting.action.ToLower = strReceiveFromImmdAction And ImmdSetting.controlFileSuffix.ToLower = strReceiveFromImmdControlFileSuffix) Then
                RunProgram = True
            Else
                RunProgram = False
            End If

            If RunProgram Then

                If SynIsToday And LocalIsToday Then
                    blnShouldRunMainFlow = False
                End If

                If SynIsToday And Not LocalIsToday Then
                    blnShouldRunMainFlow = False
                    'Update local by Syn
                    writeToFile(ImmdSetting, getFileContents(ImmdSetting, ImmdSetting.lastSuccessIndicatorSyn), ImmdSetting.lastSuccessIndicator, False)
                End If

                If Not SynIsToday And LocalIsToday Then
                    blnShouldRunMainFlow = False
                    'Update Syn By local
                    UploadLastSuccessIndicator(ImmdSetting)
                End If

                If Not SynIsToday And Not LocalIsToday Then
                    blnShouldRunMainFlow = True
                End If

            Else
                blnShouldRunMainFlow = False
                insertErrorLog(ImmdSetting, "Action and Control File Suffix Mismatch!")
            End If

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return blnShouldRunMainFlow

    End Function

    Function checkDateIsToday(ByVal ImmdSetting As Setting, ByVal strFilePath As String) As Boolean

        Dim receivedToday As Boolean = False
        Dim strLastSuccessExecutionDate As String = Left(getFileContents(ImmdSetting, strFilePath).Trim, 10)
        Dim strToday As String = Left(getFormattedDateTime(ImmdSetting, Today).Trim, 10)

        Try
            insertLog(ImmdSetting, "File path : " + strFilePath)
            insertLog(ImmdSetting, "Last success execution date : " + strLastSuccessExecutionDate)
            insertLog(ImmdSetting, "Today : " + strToday)

            If strLastSuccessExecutionDate = strToday Then
                receivedToday = True
            End If

            insertLog(ImmdSetting, "Process run already today ? " + receivedToday.ToString)

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return receivedToday

    End Function

    Function getFormattedDateTime(ByVal ImmdSetting As Setting, ByVal dt As DateTime) As String

        Dim strFormattedDateTime As String = String.Empty

        Try
            strFormattedDateTime = dt.ToString("yyyy-MM-dd HH:mm:ss")
        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return strFormattedDateTime

    End Function

    Function getFileContents(ByVal ImmdSetting As Setting, ByVal strFilePath As String) As String

        Dim strContents As String = String.Empty

        Try
            Dim objReader As StreamReader
            If File.Exists(strFilePath) Then
                objReader = New StreamReader(strFilePath)
                strContents = objReader.ReadToEnd()
                objReader.Close()
            End If
        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        Return strContents

    End Function

    Function generateFileFromTemplateFile(ByVal ImmdSetting As Setting, ByVal strFilePath As String, ByVal strTemplateFilePath As String) As Boolean

        insertLog(ImmdSetting, "Generate script file " + strFilePath + " from template file " + strTemplateFilePath)

        Dim generatedFile As Boolean = False

        Try
            ' Create an instance of StreamReader to read from a file.
            Dim sr As StreamReader = New StreamReader(strTemplateFilePath)
            Dim strTemplateline, strNewline As String
            Dim append As Boolean = False
            ' Read and display the lines from the file until the end of the file is reached.
            Do
                strTemplateline = sr.ReadLine()
                If strTemplateline IsNot Nothing Then
                    strNewline = strTemplateline

                    strNewline = strNewline.Replace("{" + Setting.CONST_eHSWebServer + "}", ImmdSetting.eHSWebServer)
                    strNewline = strNewline.Replace("{" + Setting.CONST_sectionFolder + "}", ImmdSetting.sectionFolder)
                    strNewline = strNewline.Replace("{" + Setting.CONST_scriptFolder + "}", ImmdSetting.scriptFolder)
                    strNewline = strNewline.Replace("{" + Setting.CONST_fileStore + "}", ImmdSetting.fileStore)

                    strNewline = strNewline.Replace("{" + Setting.CONST_contentFileSuffix + "}", ImmdSetting.contentFileSuffix)
                    strNewline = strNewline.Replace("{" + Setting.CONST_controlFileSuffix + "}", ImmdSetting.controlFileSuffix)

                    strNewline = strNewline.Replace("{" + Setting.CONST_get_last_success_step1_generated_file + "}", ImmdSetting.get_last_success_step1_generated_file)
                    strNewline = strNewline.Replace("{" + Setting.CONST_put_last_success_step1_generated_file + "}", ImmdSetting.put_last_success_step1_generated_file)

                    strNewline = strNewline.Replace("{" + Setting.CONST_step1_generated_file + "}", ImmdSetting.step1_generated_file)
                    strNewline = strNewline.Replace("{" + Setting.CONST_step3_generated_file + "}", ImmdSetting.step3_generated_file)
                    strNewline = strNewline.Replace("{" + Setting.CONST_step5_generated_file + "}", ImmdSetting.step5_generated_file)

                    strNewline = strNewline.Replace("{" + Setting.CONST_filePrefix + "}", ImmdSetting.filePrefix)

                    strNewline = strNewline.Replace("{" + Setting.CONST_lastSuccessIndicator + "}", Right(ImmdSetting.lastSuccessIndicator, ImmdSetting.lastSuccessIndicator.Length - ImmdSetting.sectionFolder.Length - 1))
                    strNewline = strNewline.Replace("{" + Setting.CONST_lastSuccessIndicatorSyn + "}", Right(ImmdSetting.lastSuccessIndicatorSyn, ImmdSetting.lastSuccessIndicatorSyn.Length - ImmdSetting.sectionFolder.Length - 1))

                    strNewline = strNewline.Replace("{" + Setting.CONST_ImmDFolder + "}", ImmdSetting.ImmDFolder)

                    strNewline = strNewline.Replace("{" + Setting.CONST_contentFileName + "}", ImmdSetting.contentFileName)
                    strNewline = strNewline.Replace("{" + Setting.CONST_controlFileName + "}", ImmdSetting.controlFileName)
                    strNewline = strNewline.Replace("{" + Setting.CONST_executionDate + "}", ImmdSetting.executionDate)

                    writeToFile(ImmdSetting, strNewline, strFilePath, append)
                    append = True
                End If
            Loop Until strTemplateline Is Nothing
            sr.Close()

            generatedFile = True

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

        If Not generatedFile Then
            insertLog(ImmdSetting, "Unable to generate file " + strFilePath + " from template file " + strTemplateFilePath + ". Program Terminated.")
        End If

        Return generatedFile

    End Function

    Sub writeToFile(ByVal ImmdSetting As Setting, ByVal strMessage As String, ByVal strFilePath As String, ByVal blnAppend As Boolean)

        Try
            Dim objStreamWriter As StreamWriter
            If Not File.Exists(strFilePath) Then
                objStreamWriter = New StreamWriter(File.Open(strFilePath, FileMode.OpenOrCreate))
                objStreamWriter.Close()
            End If
            objStreamWriter = New StreamWriter(strFilePath, blnAppend)
            objStreamWriter.WriteLine(strMessage)
            objStreamWriter.Close()

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

    End Sub

    Function execute(ByVal ImmdSetting As Setting, ByVal strFilePath As String)

        Dim beSuccess As Boolean = False
        Try
            insertLog(ImmdSetting, "Execute Process: " + strFilePath)
            Dim objProcess As Process = New Process()
            objProcess.StartInfo.FileName = strFilePath
            objProcess.Start()
            '5 minutes for max execute time
            objProcess.WaitForExit(300000)
            insertLog(ImmdSetting, "Process exit code : " + objProcess.ExitCode.ToString)
            If objProcess.HasExited Then
                If objProcess.ExitCode = 0 Then
                    beSuccess = True
                End If
            End If
            objProcess.Close()
        Catch ex As Exception
            insertErrorLog(ImmdSetting, ex.ToString)
        End Try
        Return beSuccess
    End Function

    Sub DeleteFile(ByVal ImmdSetting As Setting, ByVal strFilePath As String)

        Try
            If File.Exists(strFilePath) Then
                File.Delete(strFilePath)
            End If
        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try
    End Sub

    Private Sub CryptFile(ByVal password As String, ByVal in_file As String, ByVal out_file As String, ByVal encrypt As Boolean)

        ' Create input and output file streams.
        Dim in_stream As New FileStream(in_file, FileMode.Open, FileAccess.Read)
        Dim out_stream As New FileStream(out_file, FileMode.Create, FileAccess.Write)

        ' Make a triple DES service provider.
        Dim des_provider As New TripleDESCryptoServiceProvider

        ' Find a valid key size for this provider.
        Dim key_size_bits As Integer = 0
        For i As Integer = 1024 To 1 Step -1
            If des_provider.ValidKeySize(i) Then
                key_size_bits = i
                Exit For
            End If
        Next i
        Debug.Assert(key_size_bits > 0)

        ' Get the block size for this provider.
        Dim block_size_bits As Integer = des_provider.BlockSize

        ' Generate the key and initialization vector.
        Dim key As Byte() = Nothing
        Dim iv As Byte() = Nothing
        MakeKeyAndIV(password, key_size_bits, block_size_bits, key, iv)

        ' Make the encryptor or decryptor.
        Dim crypto_transform As ICryptoTransform
        If encrypt Then
            crypto_transform = des_provider.CreateEncryptor(key, iv)
        Else
            crypto_transform = des_provider.CreateDecryptor(key, iv)
        End If

        ' Attach a crypto stream to the output stream.
        Dim crypto_stream As New CryptoStream(out_stream, crypto_transform, CryptoStreamMode.Write)

        ' Encrypt or decrypt the file.
        Const BLOCK_SIZE As Integer = 1024
        Dim buffer(BLOCK_SIZE) As Byte
        Dim bytes_read As Integer
        Do
            ' Read some bytes.
            bytes_read = in_stream.Read(buffer, 0, BLOCK_SIZE)
            If bytes_read = 0 Then Exit Do

            ' Write the bytes into the CryptoStream.
            crypto_stream.Write(buffer, 0, bytes_read)
        Loop

        ' Close the streams.
        crypto_stream.Close()
        in_stream.Close()
        out_stream.Close()
    End Sub

    Private Sub MakeKeyAndIV(ByVal password As String, ByVal key_size_bits As Integer, ByVal block_size_bits As Integer, ByRef key As Byte(), ByRef iv As Byte())
        Dim password_derive_bytes As New PasswordDeriveBytes(password, Nothing, "SHA384", 1000)
        key = password_derive_bytes.GetBytes(key_size_bits \ 8)
        iv = password_derive_bytes.GetBytes(block_size_bits \ 8)
    End Sub

    Public Sub replaceClassParameters(ByVal ImmdSetting As Setting)

        Try
            Dim _type As Type = ImmdSetting.GetType()
            Dim properties() As PropertyInfo = _type.GetProperties()
            Dim strNameOld As String
            Dim strTemp As String
            Dim strNameNew As String
            Dim strValueNew As String
            Dim BeReplace As Boolean = False

            For Each _propertyOld As PropertyInfo In properties
                strNameOld = _propertyOld.Name
                strTemp = _propertyOld.GetValue(ImmdSetting, Nothing)
                BeReplace = False
                For Each _propertyNew As PropertyInfo In properties
                    strNameNew = _propertyNew.Name
                    strValueNew = _propertyNew.GetValue(ImmdSetting, Nothing)
                    If strTemp.Contains("{" + strNameNew + "}") Then
                        strTemp = strTemp.Replace("{" + strNameNew + "}", strValueNew)
                        BeReplace = True
                    End If
                Next
                If BeReplace Then
                    _propertyOld.SetValue(ImmdSetting, Convert.ChangeType(strTemp, _propertyOld.PropertyType), Nothing)
                End If
            Next

        Catch ex As Exception

            insertErrorLog(ImmdSetting, ex.ToString)

        End Try

    End Sub

    Public Function getMatchDateString(ByVal dt As DateTime) As String
        Dim strMatchDate As String = String.Empty
        Dim dtTargetDate As DateTime = dt
        strMatchDate = dtTargetDate.Year.ToString + dtTargetDate.Month.ToString.PadLeft(2, "0") + dtTargetDate.Day.ToString.PadLeft(2, "0")
        Return strMatchDate
    End Function

End Class
