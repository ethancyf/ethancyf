Imports ConnectionStringEditor.Model
Imports System.Web
Imports System.Web.Configuration
Imports System.IO
Imports System.Configuration
Imports System.Text.RegularExpressions

Namespace BLL
    Public Class ConfigFileBuilder
        Implements IDisposable
        Private m_strFilePath As String
        Private m_pGeneratorFormModel As GeneratorFormModel
        Private m_pNewConfig As System.Configuration.Configuration
        Private m_pOldConfig As System.Configuration.Configuration

        Dim disposed As Boolean = False

        Public Sub New(ByVal _model As GeneratorFormModel, ByVal _strFilePath As String)
            m_pGeneratorFormModel = _model
            m_strFilePath = _strFilePath
            Try
                m_pNewConfig = OpenConfigFile(m_pGeneratorFormModel.pNewConfigFilePath(_strFilePath))
                m_pOldConfig = OpenConfigFile(m_pGeneratorFormModel.pOldConfigFilePath(_strFilePath))
            Catch ex As Exception
                Throw New Exception("Fail!" + Environment.NewLine + "File is invalid.")
            End Try

        End Sub

        ' Public implementation of Dispose pattern callable
        Public Sub Dispose() _
                   Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ' Protected implementation of Dispose pattern. 
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposed Then Return

            If disposing Then
                ' Free any other managed objects here. 
                ' 
            End If

            ' Free any unmanaged objects here. 
            '
            disposed = True
        End Sub


        Public Function OpenConfigFile(configPath As String) As Configuration
            Try
                Dim configFile = New FileInfo(configPath)
                Dim vdm = New VirtualDirectoryMapping(configFile.DirectoryName, True, configFile.Name)
                Dim wcfm = New WebConfigurationFileMap()
                wcfm.VirtualDirectories.Add("/", vdm)
                Return WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/")
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
            Return Nothing
        End Function


        Public Function GetNewParameterList() As Dictionary(Of String, String)
            Dim resultDic = New Dictionary(Of String, String)
            For Each iii As System.Configuration.KeyValueConfigurationElement In m_pNewConfig.AppSettings.Settings
                If iii.Key.Contains(SysConst.PARAMFILTER) Then
                    resultDic.Add(iii.Key, iii.Value)
                End If
            Next
            If (resultDic.Count > 0) Then
                Return resultDic
            End If

            Return Nothing
        End Function

        Private Sub SetNewParameterWithConfigFileAPI(ByVal _strKey As String, ByVal _strValue As String)
            m_pNewConfig.AppSettings.Settings(_strKey).Value = _strValue
            m_pNewConfig.Save()
        End Sub

        Private Sub SetNewParameterWithTxtReplacementMe(ByVal _strKey As String, ByVal _strValue As String)
            Dim strConfigFileContent = File.ReadAllText(m_pNewConfig.FilePath)
            Dim strTestStringPattern = String.Format(SysConst.MATCHPARAMPATERN, _strKey)
            Dim rxPath As Regex = New Regex(strTestStringPattern, RegexOptions.IgnoreCase)
            Dim mParam As Match = rxPath.Match(strConfigFileContent)
            If (mParam.Success) Then
                File.WriteAllText(m_pNewConfig.FilePath, rxPath.Replace(strConfigFileContent, String.Format(SysConst.CONNSTREPLACE, _strKey, _strValue)))
            Else
                Throw New Exception(Nothing)
            End If

        End Sub

        Public Sub SetNewParameter(ByVal _strKey As String, ByVal _strValue As String)
            Try
                'SetNewParameterWithConfigFileAPI(_strKey, _strValue)
                SetNewParameterWithTxtReplacementMe(_strKey, _strValue)
                'm_pNewConfig.AppSettings.Settings(_strKey).Value = _strValue
                'm_pNewConfig.Save()
            Catch ex As Exception
                Throw New Exception("Fail::Parameter is missing")
            End Try
        End Sub

        Public Function GetOldParameterList() As Dictionary(Of String, String)
            Dim resultDic = New Dictionary(Of String, String)
            For Each iii As System.Configuration.KeyValueConfigurationElement In m_pOldConfig.AppSettings.Settings
                If iii.Key.Contains(SysConst.PARAMFILTER) Then
                    resultDic.Add(iii.Key, iii.Value)
                End If
            Next
            If (resultDic.Count > 0) Then
                Return resultDic
            End If

            Return Nothing
        End Function

        Public Sub SetOldParameter(ByVal _strKey As String, ByVal _strValue As String)
            Try
                m_pOldConfig.AppSettings.Settings(_strKey).Value = _strValue
                m_pOldConfig.Save()
            Catch ex As Exception
                Throw New Exception("Fail::Parameter is missing")
            End Try
        End Sub

        Public Property pGeneratorFormModel() As GeneratorFormModel
            Get
                Return m_pGeneratorFormModel
            End Get
            Set(ByVal value As GeneratorFormModel)
                m_pGeneratorFormModel = value
            End Set
        End Property
    End Class
End Namespace