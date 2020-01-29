Namespace Component.ThirdParty
    <Serializable()> Public Class ThirdPartyAdditionalFieldEnrolmentCollection
        Inherits System.Collections.Specialized.ListDictionary

        ''' <summary>
        ''' A list store sub list by source and target system
        ''' </summary>
        ''' <remarks></remarks>
        Private _listPractice As New ListDictionary()
        Private _listSysCode As New ListDictionary()

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            MyBase.Add(GenerateKey(udtModel), udtModel)

            AddByPractice(udtModel)
            AddBySysCode(udtModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            'CRE15-004 TIV & QIV [Start][Philip]
            If udtModel IsNot Nothing Then
                MyBase.Remove(GenerateKey(udtModel))

                RemoveByPractice(udtModel)
                RemoveBySysCode(udtModel)
            End If
            'CRE15-004 TIV & QIV [End][Philip]
        End Sub

        Public Overloads Sub Clear()
            MyBase.Clear()
            _listPractice.Clear()
            _listSysCode.Clear()
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal key As Object) As ThirdPartyAdditionalFieldEnrolmentModel
            Get
                Return CType(MyBase.Item(key), ThirdPartyAdditionalFieldEnrolmentModel)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="enumSourceSystem">Mapping source system</param>
        ''' <param name="enumTargetSystem">Mapping target system</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListByPractice(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, _
                                     ByVal strPracticeDisplaySeq As String) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Return GetByPractice(enumSysCode, strPracticeDisplaySeq, False)
        End Function

        Public Function GetListBySysCode(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Return GetBySysCode(enumSysCode, False)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="enumSourceSystem">Mapping source system</param>
        ''' <param name="enumTargetSystem">Mapping target system</param>
        ''' <param name="strCodeType">Vaccine code from source system</param>
        ''' <returns>Vaccine code mapping model for retrieve target vaccine code and display name</returns>
        ''' <remarks></remarks>
        Public Function GetByValueCode(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, _
                                     ByVal strPracticeDisplaySeq As String, _
                                     ByVal strAdditionalFieldID As String) As ThirdPartyAdditionalFieldEnrolmentModel
            Return CType(MyBase.Item(GenerateKey(enumSysCode, strPracticeDisplaySeq, strAdditionalFieldID)), ThirdPartyAdditionalFieldEnrolmentModel)
        End Function


#Region "Private Function"

#Region "By Practice"

        Private Sub AddByPractice(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyByPractice(udtModel)

            Dim listPractice As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = GetByPractice(udtModel, True)

            If Not listPractice.Contains(udtModel) Then
                listPractice.Add(udtModel)
            End If
        End Sub

        Private Sub RemoveByPractice(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyByPractice(udtModel)

            Dim listPractice As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = GetByPractice(udtModel, False)

            If listPractice Is Nothing Then Exit Sub

            If listPractice.Contains(udtModel) Then
                listPractice.Remove(udtModel)
            End If
        End Sub

        Private Function GetByPractice(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel, ByVal bCreateIfNotExist As Boolean) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Return GetByPractice(udtModel.SysCode, udtModel.PracticeDisplaySeq, bCreateIfNotExist)
        End Function


        Private Function GetByPractice(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, _
                                     ByVal strPracticeDisplaySeq As String, ByVal bCreateIfNotExist As Boolean) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyByPractice(enumSysCode, strPracticeDisplaySeq)

            Dim listPractice As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = Nothing

            If _listPractice.Contains(strKey) Then
                listPractice = _listPractice(strKey)
            Else
                listPractice = New List(Of ThirdPartyAdditionalFieldEnrolmentModel)
                If bCreateIfNotExist Then
                    _listPractice.Add(strKey, listPractice)
                End If
            End If

            Return listPractice
        End Function

        Private Function GenerateKeyByPractice(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel) As String
            Return GenerateKeyByPractice(udtModel.SysCode, _
                                        udtModel.PracticeDisplaySeq)
        End Function

        Private Function GenerateKeyByPractice(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, _
                                                 ByVal strPracticeDisplaySeq As String) As String
            Return enumSysCode.ToString() + "|" + "|" + strPracticeDisplaySeq
        End Function

#End Region


#Region "By Sys Code"

        Private Sub AddBySysCode(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyBySysCode(udtModel)

            Dim listSysCode As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = GetBySysCode(udtModel, True)

            If Not listSysCode.Contains(udtModel) Then
                listSysCode.Add(udtModel)
            End If
        End Sub

        Private Sub RemoveBySysCode(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyByPractice(udtModel)

            Dim listSysCode As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = GetBySysCode(udtModel, False)

            If listSysCode Is Nothing Then Exit Sub

            If listSysCode.Contains(udtModel) Then
                listSysCode.Remove(udtModel)
            End If
        End Sub

        Private Function GetBySysCode(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel, ByVal bCreateIfNotExist As Boolean) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Return GetBySysCode(udtModel.SysCode, bCreateIfNotExist)
        End Function


        Private Function GetBySysCode(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, ByVal bCreateIfNotExist As Boolean) As List(Of ThirdPartyAdditionalFieldEnrolmentModel)
            Dim strKey As String = GenerateKeyBySysCode(enumSysCode)

            Dim listSysCode As List(Of ThirdPartyAdditionalFieldEnrolmentModel) = Nothing

            If _listSysCode.Contains(strKey) Then
                listSysCode = _listSysCode(strKey)
            Else
                listSysCode = New List(Of ThirdPartyAdditionalFieldEnrolmentModel)
                If bCreateIfNotExist Then
                    _listSysCode.Add(strKey, listSysCode)
                End If
            End If

            Return listSysCode
        End Function

        Private Function GenerateKeyBySysCode(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel) As String
            Return GenerateKeyBySysCode(udtModel.SysCode)
        End Function

        Private Function GenerateKeyBySysCode(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode) As String
            Return enumSysCode.ToString()
        End Function

#End Region

        Private Function GenerateKey(ByVal udtModel As ThirdPartyAdditionalFieldEnrolmentModel) As String
            Return GenerateKey(udtModel.SysCode, _
                                udtModel.PracticeDisplaySeq, _
                                udtModel.AdditionalFieldID)
        End Function

        Private Function GenerateKey(ByVal enumSysCode As ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode, _
                                     ByVal strPracticeDisplaySeq As String, _
                                     ByVal strAdditionalFieldID As String) As String
            Return enumSysCode.ToString() + "|" + "|" + strPracticeDisplaySeq + "|" + strAdditionalFieldID
        End Function

#End Region
    End Class
End Namespace

