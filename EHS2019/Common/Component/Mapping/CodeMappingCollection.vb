Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Component.Mapping
    <Serializable()> Public Class CodeMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        ''' <summary>
        ''' A list store sub list by source and target system
        ''' </summary>
        ''' <remarks></remarks>
        Private _listSystem As New ListDictionary()

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtCodeMappingModel As CodeMappingModel)
            MyBase.Add(GenerateKey(udtCodeMappingModel), udtCodeMappingModel)

            AddByCodeType(udtCodeMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtCodeMappingModel As CodeMappingModel)
            MyBase.Remove(GenerateKey(udtCodeMappingModel))

            RemoveByCodeType(udtCodeMappingModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal key As Object) As CodeMappingModel
            Get
                Return CType(MyBase.Item(key), CodeMappingModel)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="enumSourceSystem">Mapping source system</param>
        ''' <param name="enumTargetSystem">Mapping target system</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListByCodeType(ByVal enumSourceSystem As CodeMappingModel.EnumSourceSystem, _
                                          ByVal enumTargetSystem As CodeMappingModel.EnumTargetSystem, _
                                          ByVal strCodeType As String) As List(Of CodeMappingModel)
            Return GetByCodeType(enumSourceSystem, enumTargetSystem, strCodeType, False)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="enumSourceSystem">Mapping source system</param>
        ''' <param name="enumTargetSystem">Mapping target system</param>
        ''' <param name="strCodeType">Vaccine code from source system</param>
        ''' <returns>Vaccine code mapping model for retrieve target vaccine code and display name</returns>
        ''' <remarks></remarks>
        Public Function GetMappingByCode(ByVal enumSourceSystem As CodeMappingModel.EnumSourceSystem, _
                                         ByVal enumTargetSystem As CodeMappingModel.EnumTargetSystem, _
                                         ByVal strCodeType As String, ByVal strCodeSource As String) As CodeMappingModel
            Return CType(MyBase.Item(GenerateKey(enumSourceSystem, enumTargetSystem, strCodeType, strCodeSource)), CodeMappingModel)
        End Function

#Region "Private Function"

        Private Sub AddByCodeType(ByVal udtCodeMappingModel As CodeMappingModel)
            Dim strKey As String = GenerateKeyByCodeType(udtCodeMappingModel)

            Dim listSystem As List(Of CodeMappingModel) = GetByCodeType(udtCodeMappingModel, True)

            If Not listSystem.Contains(udtCodeMappingModel) Then
                listSystem.Add(udtCodeMappingModel)
            End If
        End Sub

        Private Sub RemoveByCodeType(ByVal udtCodeMappingModel As CodeMappingModel)
            Dim strKey As String = GenerateKeyByCodeType(udtCodeMappingModel)

            Dim listSystem As List(Of CodeMappingModel) = GetByCodeType(udtCodeMappingModel, False)

            If listSystem Is Nothing Then Exit Sub

            If listSystem.Contains(udtCodeMappingModel) Then
                listSystem.Remove(udtCodeMappingModel)
            End If
        End Sub

        Private Function GetByCodeType(ByVal udtCodeMappingModel As CodeMappingModel, ByVal bCreateIfNotExist As Boolean) As List(Of CodeMappingModel)
            Return GetByCodeType(udtCodeMappingModel.SourceSystem, udtCodeMappingModel.TargetSystem, udtCodeMappingModel.CodeType, bCreateIfNotExist)
        End Function

        Private Function GetByCodeType(ByVal enumSourceSystem As CodeMappingModel.EnumSourceSystem, _
                                       ByVal enumTargetSystem As CodeMappingModel.EnumTargetSystem, _
                                       ByVal strCodeType As String, ByVal bCreateIfNotExist As Boolean) As List(Of CodeMappingModel)
            Dim strKey As String = GenerateKeyByCodeType(enumSourceSystem, enumTargetSystem, strCodeType)

            Dim listSystem As List(Of CodeMappingModel) = Nothing

            If _listSystem.Contains(strKey) Then
                listSystem = _listSystem(strKey)
            Else
                If bCreateIfNotExist Then
                    listSystem = New List(Of CodeMappingModel)
                    _listSystem.Add(strKey, listSystem)
                End If
            End If

            Return listSystem
        End Function

        Private Function GenerateKeyByCodeType(ByVal udtCodeMappingModel As CodeMappingModel) As String
            Return GenerateKeyByCodeType(udtCodeMappingModel.SourceSystem, _
                                udtCodeMappingModel.TargetSystem, _
                                udtCodeMappingModel.CodeType)
        End Function

        Private Function GenerateKeyByCodeType(ByVal enumSourceSystem As CodeMappingModel.EnumSourceSystem, _
                                               ByVal enumTargetSystem As CodeMappingModel.EnumTargetSystem, _
                                               ByVal strCodeType As String) As String
            Return enumSourceSystem.ToString() + "|" + enumTargetSystem.ToString() + "|" + strCodeType
        End Function

        Private Function GenerateKey(ByVal udtCodeMappingModel As CodeMappingModel) As String
            Return GenerateKey(udtCodeMappingModel.SourceSystem, _
                                udtCodeMappingModel.TargetSystem, _
                                udtCodeMappingModel.CodeType, _
                                udtCodeMappingModel.CodeSource)
        End Function

        Private Function GenerateKey(ByVal enumSourceSystem As CodeMappingModel.EnumSourceSystem, _
                                     ByVal enumTargetSystem As CodeMappingModel.EnumTargetSystem, _
                                     ByVal strCodeType As String, ByVal strCodeSource As String) As String
            Return enumSourceSystem.ToString() + "|" + enumTargetSystem.ToString() + "|" + strCodeType + "|" + strCodeSource
        End Function

#End Region
    End Class
End Namespace

