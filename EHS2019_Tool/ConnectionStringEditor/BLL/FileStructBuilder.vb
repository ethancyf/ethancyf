Imports System.IO
Imports ConnectionStringEditor.Model

Namespace BLL
    Public Class FileStructBuilder
        Public Sub ScanDir(ByRef _pGeneratorFormModel As GeneratorFormModel, ByVal _DirPath As String)
            Dim strFilePath As String = String.Empty
            _pGeneratorFormModel.pFileList.Clear()

            Dim tmpFileName As String = Nothing
            Dim strConfigExe As String = SysConst.READCONFIGFILEEXT
            For Each ddd As String In _pGeneratorFormModel.pEHSProjectFileMappingList
                strFilePath = ClarifyDirectoryPath(_DirPath) + ClarifyDirectoryPath(ddd)
                If (Directory.Exists(strFilePath)) Then
                    For Each fff As String In Directory.GetFiles(strFilePath)
                        tmpFileName = Path.GetFileName(fff).ToLower()
                        If tmpFileName = SysConst.READCONFIGFILENAME.ToLower() Then
                            _pGeneratorFormModel.pFileList.Add(fff)
                        ElseIf (tmpFileName.Length > strConfigExe.Length) Then
                            If (tmpFileName.Substring(tmpFileName.Length - strConfigExe.Length, strConfigExe.Length).ToLower() = strConfigExe.ToLower()) Then
                                _pGeneratorFormModel.pFileList.Add(fff)
                            End If
                        End If

                    Next
                End If
            Next
        End Sub


        Public Function ClarifyDirectoryPath(ByVal _DirPath As String) As String
            _DirPath = RTrim(LTrim(_DirPath))
            If (Not _DirPath.EndsWith("\")) Then
                _DirPath = _DirPath + "\"
            End If
            Return _DirPath
        End Function

        Public Sub GenChangeList(ByVal _pGeneratorFormModel As GeneratorFormModel, ByVal _DirPath As String, ByVal _fileName As String)
            File.WriteAllLines(ClarifyDirectoryPath(_DirPath) + _fileName, _pGeneratorFormModel.pFileList)
        End Sub

        Public Sub GenFolderStruct(ByRef _pGeneratorFormModel As GeneratorFormModel, ByVal _DirPath As String, ByVal _fileName As String)
            _pGeneratorFormModel.pNewConfigFilePath.Clear()
            _pGeneratorFormModel.pOldConfigFilePath.Clear()

            For Each oldfilePath As String In _pGeneratorFormModel.pFileList
                Dim newFile_OldConfigFile_Path = oldfilePath.Replace(_pGeneratorFormModel.strScanFolderPath, _DirPath + _pGeneratorFormModel.strOldCopyPath)
                Dim newFile_NewConfigFile_Path = oldfilePath.Replace(_pGeneratorFormModel.strScanFolderPath, _DirPath + _pGeneratorFormModel.strNewCopyPath)

                Dim newFile_OldConfigFile As FileInfo = New System.IO.FileInfo(newFile_OldConfigFile_Path)
                newFile_OldConfigFile.Directory.Create()
                Dim newFile_NewConfigFile As FileInfo = New System.IO.FileInfo(newFile_NewConfigFile_Path)
                newFile_OldConfigFile.Directory.Create()
                newFile_NewConfigFile.Directory.Create()

                Dim oldFile As FileInfo = New System.IO.FileInfo(oldfilePath)

                CopyServerFile(oldFile, newFile_OldConfigFile_Path)
                CopyServerFile(oldFile, newFile_NewConfigFile_Path)

                _pGeneratorFormModel.pNewConfigFilePath.Add(oldfilePath, newFile_NewConfigFile_Path)
                _pGeneratorFormModel.pOldConfigFilePath.Add(oldfilePath, newFile_OldConfigFile_Path)
            Next
        End Sub

        Private Sub CopyServerFile(ByVal pSourceFile As FileInfo, ByVal strTargetFile As String)
            If (File.Exists(strTargetFile)) Then
                File.Delete(strTargetFile)
            End If
            pSourceFile.CopyTo(strTargetFile)
        End Sub


        Public Sub RemoveOrCreateDirectory(ByVal _strDirPath As String)
            If (Directory.Exists(_strDirPath)) Then
                Dim tmpDirInfo As DirectoryInfo = New DirectoryInfo(_strDirPath)

                For Each file As FileInfo In tmpDirInfo.GetFiles()
                    Try
                        file.Delete()
                    Catch ex As Exception

                    End Try
                Next

                For Each dir As DirectoryInfo In tmpDirInfo.GetDirectories()
                    Try
                        dir.Delete(True)
                    Catch ex As Exception

                    End Try
                Next
            Else
                Directory.CreateDirectory(_strDirPath)
            End If
        End Sub

        Public Sub FillInConfigFilePathsForDic(ByRef _pGeneratorFormModel As GeneratorFormModel)
            _pGeneratorFormModel.pWebConfigDic.Clear()
            For Each tmpStr As String In _pGeneratorFormModel.pFileList
                Try
                    _pGeneratorFormModel.pWebConfigDic.Add(tmpStr, Nothing)
                Catch ex As Exception

                End Try
            Next
        End Sub
    End Class
End Namespace
