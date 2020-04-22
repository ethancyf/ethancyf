Imports System.IO
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports ConnectionStringEditor.Model

Namespace BLL
    Public Class LoadAppSetting
        Private Shared m_pLoadAppSetting As LoadAppSetting
        Private Shared m_pConfig As Configuration

        Public Shared Function GetInstance() As LoadAppSetting
            If (m_pLoadAppSetting Is Nothing) Then
                m_pLoadAppSetting = New LoadAppSetting()
            End If
            Return m_pLoadAppSetting
        End Function

        Public Sub LoadSettingFile(ByRef _pGeneratorFormModel As GeneratorFormModel, ByVal _folderScanPath As String, ByVal _settingFilePath As String)
            _pGeneratorFormModel.strScanFolderPath = _folderScanPath
            _pGeneratorFormModel.strOldCopyPath = GetMySettingValues("OldConfigFile")
            _pGeneratorFormModel.strNewCopyPath = GetMySettingValues("NewConfigFile")
            _pGeneratorFormModel.strEncryptedKey = GetMySettingValues("EncryptKey")
            _pGeneratorFormModel.strDecryptedKey = GetMySettingValues("DecryptKey")

            _pGeneratorFormModel.pSuffixArr = m_pLoadAppSetting.GetAppSetting("Suffix", _settingFilePath)
            _pGeneratorFormModel.pEHSProjectFileMappingList = m_pLoadAppSetting.GetEHSProjectFileMappingArr("EHSProjectFileMapping", GetMySettingValues("ProjectWithoutConnectionString"), _settingFilePath)
        End Sub


        Public Function GetAppSetting(ByVal _key As String, ByVal _filePath As String) As String()
            Try
                Dim map As ExeConfigurationFileMap = New ExeConfigurationFileMap()
                map.ExeConfigFilename = _filePath
                Dim pConfig As Configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None)
                Dim strResult As String = pConfig.AppSettings().Settings(_key).Value.ToString()
                Dim pResultArr As String() = strResult.Split(New Char() {"|", Environment.NewLine})
                Return pResultArr
            Catch ex As Exception
            End Try
            Return Nothing
        End Function

        Public Function GetEHSProjectFileMappingArr(ByVal _key As String, ByVal _strConnectionStringNoChanges As String, _filePath As String) As List(Of String)
            Try
                Dim _EHSProjectFileMappingArr As String() = GetAppSetting(_key, _filePath)
                Dim resultDic As List(Of String) = New List(Of String)
                Dim pConnectionStringNoChanges As String() = _strConnectionStringNoChanges.Split(New Char() {"|", Environment.NewLine})

                For Each tmpStr As String In _EHSProjectFileMappingArr
                    If Not String.IsNullOrEmpty(tmpStr) And Not String.IsNullOrWhiteSpace(tmpStr) Then
                        Dim tmpArr As String() = tmpStr.Split(New Char() {":", Environment.NewLine})
                        Dim tmpNoChangesList = From ttt As String In pConnectionStringNoChanges Where ttt.Trim().ToLower() = tmpArr(0).Trim().ToLower()
                        If (tmpNoChangesList.Count > 0) Then
                            Continue For
                        End If
                        resultDic.Add(tmpArr(1).Trim())
                    End If
                Next
                resultDic.Sort()
                Return resultDic
            Catch ex As Exception

            End Try
            Return Nothing
        End Function

        Public Sub LoadAppConfigSetting()
            Dim map As ExeConfigurationFileMap = New ExeConfigurationFileMap()
            map.ExeConfigFilename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            m_pConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None)
        End Sub

        Public Function GetMySettingValues(ByVal _key As String) As String
            Try
                If (Not String.IsNullOrEmpty(_key) AndAlso Not (m_pConfig Is Nothing)) Then
                    Return m_pConfig.AppSettings().Settings(_key).Value
                End If
            Catch ex As Exception

            End Try
            Return String.Empty
        End Function

        Public Function GetMySettingValues(ByVal _key As String, ByVal _filePath As String) As String
            Try
                Dim map As ExeConfigurationFileMap = New ExeConfigurationFileMap()
                map.ExeConfigFilename = _filePath
                Dim pConfig As Configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None)
                Return pConfig.AppSettings().Settings(_key).Value.ToString()
            Catch ex As Exception
            End Try
            Return Nothing
        End Function

        Public Sub SetMySettingValues(ByVal _key As String, ByVal _value As String)
            m_pConfig.AppSettings().Settings(_key).Value = _value
            m_pConfig.Save()
        End Sub

        Public Sub SetMySettingValues(ByVal _key As String, ByVal _value As String, ByVal _filePath As String)
            Try
                Dim map As ExeConfigurationFileMap = New ExeConfigurationFileMap()
                map.ExeConfigFilename = _filePath
                Dim pConfig As Configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None)
                pConfig.AppSettings().Settings(_key).Value = _value
                pConfig.Save()
            Catch ex As Exception
            End Try
        End Sub


    End Class
End Namespace