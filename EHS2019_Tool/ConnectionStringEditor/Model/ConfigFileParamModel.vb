Namespace Model
    Public Class ConnectionStringModel
        Implements ICloneable

        Private m_strConfigFilePath As String
        Private m_strParamName As String

        Private m_strNewParamRawValue As String
        Private m_strOldParamRawValue As String

        Private m_strDecryptedParamValue As String

        Private m_strUsername As String
        Private m_strUserPwd As String
        Private m_strDBServerName As String
        Private m_strDBInstanceName As String

        Private m_strPoolSize As String
        Private m_strEncryptKey As String

        Private m_strEncryptResult As String
        Private m_strDecryptedResult As String


        Private m_strEncryptedKey As String
        Private m_strDecryptedKey As String

        Private m_bApplyToAll As Boolean
        Private m_bVerify As Boolean



        Public Property bVerify() As Boolean
            Get
                Return m_bVerify
            End Get
            Set(ByVal value As Boolean)
                m_bVerify = value
            End Set
        End Property



        Public Function Clone() As Object Implements ICloneable.Clone
            Dim typClone As ConnectionStringModel = Me.MemberwiseClone
            Try
                If Me.m_strConfigFilePath IsNot Nothing Then
                    typClone.m_strConfigFilePath = Me.m_strConfigFilePath.Clone
                End If
                If Me.m_strParamName IsNot Nothing Then
                    typClone.m_strParamName = Me.m_strParamName.Clone
                End If
                If Me.m_strNewParamRawValue IsNot Nothing Then
                    typClone.m_strNewParamRawValue = Me.m_strNewParamRawValue.Clone
                End If

                If Me.m_strOldParamRawValue IsNot Nothing Then
                    typClone.m_strOldParamRawValue = Me.m_strOldParamRawValue.Clone
                End If

                If Me.m_strDecryptedParamValue IsNot Nothing Then
                    typClone.m_strDecryptedParamValue = Me.m_strDecryptedParamValue.Clone
                End If

                If Me.m_strUsername IsNot Nothing Then
                    typClone.m_strUsername = Me.m_strUsername.Clone
                End If

                If Me.m_strUserPwd IsNot Nothing Then
                    typClone.m_strUserPwd = Me.m_strUserPwd.Clone
                End If

                If Me.m_strDBServerName IsNot Nothing Then
                    typClone.m_strDBServerName = Me.m_strDBServerName.Clone
                End If

                If Me.m_strDBInstanceName IsNot Nothing Then
                    typClone.m_strDBInstanceName = Me.m_strDBInstanceName.Clone
                End If

                If Me.m_strPoolSize IsNot Nothing Then
                    typClone.m_strPoolSize = Me.m_strPoolSize.Clone
                End If

                If Me.m_strEncryptKey IsNot Nothing Then
                    typClone.m_strEncryptKey = Me.m_strEncryptKey.Clone
                End If

                If Me.m_strEncryptResult IsNot Nothing Then
                    typClone.m_strEncryptResult = Me.m_strEncryptResult.Clone
                End If

                If Me.m_strEncryptedKey IsNot Nothing Then
                    typClone.m_strEncryptedKey = Me.m_strEncryptedKey.Clone
                End If

                If Me.m_strDecryptedKey IsNot Nothing Then
                    typClone.m_strDecryptedKey = Me.m_strDecryptedKey.Clone
                End If

                typClone.m_bApplyToAll = Me.m_bApplyToAll
                typClone.m_bVerify = Me.m_bVerify

            Catch ex As Exception
            End Try
            Return typClone

        End Function

        Public Property strDecryptedResult() As String
            Get
                Return m_strDecryptedResult
            End Get
            Set(ByVal value As String)
                m_strDecryptedResult = value
            End Set
        End Property

        Public Property strDBInstanceName() As String
            Get
                Return m_strDBInstanceName
            End Get
            Set(ByVal value As String)
                m_strDBInstanceName = value
            End Set
        End Property

        Public Property bApplyToAll() As Boolean
            Get
                Return m_bApplyToAll
            End Get
            Set(ByVal value As Boolean)
                m_bApplyToAll = value
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

        Public Property strParamName() As String
            Get
                Return m_strParamName
            End Get
            Set(ByVal value As String)
                m_strParamName = value
            End Set
        End Property

        Public Property strNewParamRawValue() As String
            Get
                Return m_strNewParamRawValue
            End Get
            Set(ByVal value As String)
                m_strNewParamRawValue = value
            End Set
        End Property


        Public Property strConfigFilePath() As String
            Get
                Return m_strConfigFilePath
            End Get
            Set(ByVal value As String)
                m_strConfigFilePath = value
            End Set
        End Property


        Public Property strUsername() As String
            Get
                Return m_strUsername
            End Get
            Set(ByVal value As String)
                m_strUsername = value
            End Set
        End Property

        Public Property strUserPwd() As String
            Get
                Return m_strUserPwd
            End Get
            Set(ByVal value As String)
                m_strUserPwd = value
            End Set
        End Property

        Public Property strDBServerName() As String
            Get
                Return m_strDBServerName
            End Get
            Set(ByVal value As String)
                m_strDBServerName = value
            End Set
        End Property

        Public Property strPoolSize() As String
            Get
                Return m_strPoolSize
            End Get
            Set(ByVal value As String)
                m_strPoolSize = value
            End Set
        End Property

        Public Property strEncryptKey() As String
            Get
                Return m_strEncryptKey
            End Get
            Set(ByVal value As String)
                m_strEncryptKey = value
            End Set
        End Property

        Public Property strDecryptedParamValue() As String
            Get
                Return m_strDecryptedParamValue
            End Get
            Set(ByVal value As String)
                m_strDecryptedParamValue = value
            End Set
        End Property

        Public Property strEncryptResult() As String
            Get
                Return m_strEncryptResult
            End Get
            Set(ByVal value As String)
                m_strEncryptResult = value
            End Set
        End Property

        Public Property strOldParamRawValue() As String
            Get
                Return m_strOldParamRawValue
            End Get
            Set(ByVal value As String)
                m_strOldParamRawValue = value
            End Set
        End Property
    End Class
End Namespace