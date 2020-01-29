Imports System.Collections.Generic

Namespace Model
    Public Class GeneratorFormModel

        Private m_pSuffixArr As String()

        Private m_strScanFolderPath As String
        Private m_strOldCopyPath As String
        Private m_strNewCopyPath As String

        Private m_pFileList = New List(Of String)
        Private m_pEHSProjectFileMappingList = New List(Of String)

        Private m_pWebConfigDic = New Dictionary(Of String, Dictionary(Of String, ConnectionStringModel))

        Private m_pNewConfigFilePath As Dictionary(Of String, String)
        Private m_pOldConfigFilePath As Dictionary(Of String, String)

        Private m_strEncryptedKey As String
        Private m_strDecryptedKey As String

        Public Property pSuffixArr() As String()
            Get
                Return m_pSuffixArr
            End Get
            Set(ByVal value As String())
                m_pSuffixArr = value
            End Set
        End Property

        Public Property strDecryptedKey() As String
            Get
                Return m_strDecryptedKey
            End Get
            Set(ByVal value As String)
                m_strDecryptedKey = value
            End Set
        End Property

        Public Property strEncryptedKey() As String
            Get
                Return m_strEncryptedKey
            End Get
            Set(ByVal value As String)
                m_strEncryptedKey = value
            End Set
        End Property

        Public Property pOldConfigFilePath() As Dictionary(Of String, String)
            Get
                If (m_pOldConfigFilePath Is Nothing) Then
                    m_pOldConfigFilePath = New Dictionary(Of String, String)
                End If
                Return m_pOldConfigFilePath
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                m_pOldConfigFilePath = value
            End Set
        End Property

        Public Property pNewConfigFilePath() As Dictionary(Of String, String)
            Get
                If (m_pNewConfigFilePath Is Nothing) Then
                    m_pNewConfigFilePath = New Dictionary(Of String, String)
                End If
                Return m_pNewConfigFilePath
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                m_pNewConfigFilePath = value
            End Set
        End Property


        Public Property strScanFolderPath() As String
            Get
                Return m_strScanFolderPath
            End Get
            Set(ByVal value As String)
                m_strScanFolderPath = value
            End Set
        End Property

        Public Property strOldCopyPath() As String
            Get
                Return m_strOldCopyPath
            End Get
            Set(ByVal value As String)
                m_strOldCopyPath = value
            End Set
        End Property

        Public Property strNewCopyPath() As String
            Get
                Return m_strNewCopyPath
            End Get
            Set(ByVal value As String)
                m_strNewCopyPath = value
            End Set
        End Property


        Public Property pWebConfigDic() As Dictionary(Of String, Dictionary(Of String, ConnectionStringModel))
            Get
                Return m_pWebConfigDic
            End Get
            Set(ByVal value As Dictionary(Of String, Dictionary(Of String, ConnectionStringModel)))
                m_pWebConfigDic = value
            End Set
        End Property


        Public Property pFileList() As List(Of String)
            Get
                Return m_pFileList
            End Get
            Set(ByVal value As List(Of String))
                m_pFileList = value
            End Set
        End Property

        Public Property pEHSProjectFileMappingList() As List(Of String)
            Get
                Return m_pEHSProjectFileMappingList
            End Get
            Set(ByVal value As List(Of String))
                m_pEHSProjectFileMappingList = value
            End Set
        End Property

        Public Sub LoadEHSProjectFileMappingList(ByVal _suffix As String)
            For iii As Integer = 0 To m_pEHSProjectFileMappingList.Count - 1
                m_pEHSProjectFileMappingList(iii) = m_pEHSProjectFileMappingList(iii).Replace("{suffix}", _suffix)
            Next
        End Sub
    End Class
End Namespace
