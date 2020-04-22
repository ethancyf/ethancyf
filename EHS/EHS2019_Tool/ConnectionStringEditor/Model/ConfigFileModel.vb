Public Class WebConfigFileModel

    Private m_pNewConnectionStringDir As Dictionary(Of String, String)
    Private m_pOldConnectionStringDir As Dictionary(Of String, String)


    Public Sub New()
        m_pNewConnectionStringDir = New Dictionary(Of String, String)
        m_pOldConnectionStringDir = New Dictionary(Of String, String)
    End Sub


    Public Property pNewConnectionStringDir() As Dictionary(Of String, String)
        Get
            Return m_pNewConnectionStringDir
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            m_pNewConnectionStringDir = value
        End Set
    End Property

    Public Property pOldConnectionStringDir() As Dictionary(Of String, String)
        Get
            Return m_pOldConnectionStringDir
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            m_pOldConnectionStringDir = value
        End Set
    End Property

End Class
