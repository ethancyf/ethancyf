Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Component.DHTransaction
    <Serializable()> Public Class HKMTTVaccineSeasonMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        ''' <summary>
        ''' A list store sub list by source and target system
        ''' </summary>
        ''' <remarks></remarks>
        Private _listSystem As New ListDictionary()

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel)
            MyBase.Add(udtHKMTTVaccineSeasonMappingModel.GenerateKey(), udtHKMTTVaccineSeasonMappingModel)

            AddBySystem(udtHKMTTVaccineSeasonMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel)
            MyBase.Remove(udtHKMTTVaccineSeasonMappingModel.GenerateKey)

            RemoveBySystem(udtHKMTTVaccineSeasonMappingModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As HKMTTVaccineSeasonMappingModel
            Get
                Return CType(MyBase.Item(intIndex), HKMTTVaccineSeasonMappingModel)
            End Get
        End Property

        Public Function GetMapping(ByVal udtHKMTTVaccineMapping As HKMTTVaccineMappingModel, ByVal udtDHVaccineModel As DHVaccineModel) As HKMTTVaccineSeasonMappingModel
            Dim listSystem As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel) = GetBySystem(udtHKMTTVaccineMapping.SourceSystem, _
                                                                                                    udtHKMTTVaccineMapping.TargetSystem, _
                                                                                                    udtHKMTTVaccineMapping.VaccineTypeTarget, _
                                                                                                    True)

            Dim udtResult As HKMTTVaccineSeasonMappingModel = Nothing

            ' Vaccine mapping is not found
            If listSystem.Count = 0 Then
                Throw New Exception(String.Format("HKMTTVaccineSeasonMapping is not found [SourceSystem={0}, TargetSystem={1}, VaccineType={2}]", udtHKMTTVaccineMapping.SourceSystem, _
                                                  udtHKMTTVaccineMapping.TargetSystem, _
                                                  udtHKMTTVaccineMapping.VaccineTypeTarget))
            End If

            ' Search vaccine season by DH administration date
            For i As Integer = 0 To listSystem.Count - 1
                If listSystem.Keys(i) > udtDHVaccineModel.AdmDate Then
                    If i = 0 Then
                        Throw New Exception(String.Format("DH administration date is earlier than HKMTTVaccineSeasonMapping setting [SourceSystem={0}, TargetSystem={1}, VaccineType={2}, Administration Date={3}]", udtHKMTTVaccineMapping.SourceSystem, _
                                                    udtHKMTTVaccineMapping.TargetSystem, _
                                                    udtHKMTTVaccineMapping.VaccineTypeTarget, _
                                                    udtDHVaccineModel.AdmDate.ToString))
                    End If

                    udtResult = listSystem.Values(i - 1)
                    Exit For
                End If
            Next

            ' Vaccine season mapping is not found, map to latest season
            If udtResult Is Nothing Then
                udtResult = listSystem.Values(listSystem.Count - 1)
                'Throw New Exception(String.Format("HKMTTVaccineSeasonMapping is not found [SourceSystem={0}, TargetSystem={1}, VaccineType={2}, Administration Date={3}]", udtHKMTTVaccineMapping.SourceSystem, _
                '                                     udtHKMTTVaccineMapping.TargetSystem, _
                '                                     udtHKMTTVaccineMapping.VaccineTypeTarget, _
                '                                     udtDHVaccineModel.AdmDate.ToString))
            End If

            Return udtResult
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSourceSystem">Mapping source system, constant value from class VaccineCodeMappingModel.SourceSystemClass</param>
        ''' <param name="strTargetSystem">Mapping target system, constant value from class VaccineCodeMappingModel.TargetSystemClass</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strVaccineTypeSource As String) As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel)
            Return GetBySystem(strSourceSystem, strTargetSystem, strVaccineTypeSource, False)
        End Function

#Region "Private Function"

        Private Sub AddBySystem(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel)
            Dim strKey As String = udtHKMTTVaccineSeasonMappingModel.GenerateKeyBySystem

            Dim listSystem As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel) = GetBySystem(udtHKMTTVaccineSeasonMappingModel, True)

            If Not listSystem.ContainsValue(udtHKMTTVaccineSeasonMappingModel) Then
                listSystem.Add(udtHKMTTVaccineSeasonMappingModel.InjectionDtmFromSource, udtHKMTTVaccineSeasonMappingModel)
            End If
        End Sub

        Private Sub RemoveBySystem(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel)
            Dim strKey As String = udtHKMTTVaccineSeasonMappingModel.GenerateKeyBySystem

            Dim listSystem As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel) = GetBySystem(udtHKMTTVaccineSeasonMappingModel, False)

            If listSystem Is Nothing Then Exit Sub

            If listSystem.ContainsValue(udtHKMTTVaccineSeasonMappingModel) Then
                listSystem.Remove(udtHKMTTVaccineSeasonMappingModel.InjectionDtmFromSource)
            End If
        End Sub

        Private Function GetBySystem(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel, ByVal bCreateIfNotExist As Boolean) As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel)
            Return GetBySystem(udtHKMTTVaccineSeasonMappingModel.SourceSystem, _
                               udtHKMTTVaccineSeasonMappingModel.TargetSystem, _
                               udtHKMTTVaccineSeasonMappingModel.VaccineTypeSource, _
                               bCreateIfNotExist)
        End Function

        Private Function GetBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strVaccineTypeSource As String, ByVal bCreateIfNotExist As Boolean) As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel)
            Dim strKey As String = HKMTTVaccineSeasonMappingModel.GenerateKeyBySystem(strSourceSystem, strTargetSystem, strVaccineTypeSource)

            Dim listSystem As SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel) = Nothing

            If _listSystem.Contains(strKey) Then
                listSystem = _listSystem(strKey)
            Else
                If bCreateIfNotExist Then
                    listSystem = New SortedList(Of DateTime, HKMTTVaccineSeasonMappingModel)
                    _listSystem.Add(strKey, listSystem)
                End If
            End If

            Return listSystem
        End Function

#End Region
    End Class
End Namespace

