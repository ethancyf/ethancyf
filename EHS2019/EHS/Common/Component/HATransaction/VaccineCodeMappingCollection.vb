Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Component.HATransaction
    <Serializable()> Public Class VaccineCodeMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        ''' <summary>
        ''' A list store sub list by source and target system
        ''' </summary>
        ''' <remarks></remarks>
        Private _listSystem As New ListDictionary()

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel)
            MyBase.Add(GenerateKey(udtVaccineCodeMappingModel), udtVaccineCodeMappingModel)

            AddBySystem(udtVaccineCodeMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel)
            MyBase.Remove(GenerateKey(udtVaccineCodeMappingModel))

            RemoveBySystem(udtVaccineCodeMappingModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VaccineCodeMappingModel
            Get
                Return CType(MyBase.Item(intIndex), VaccineCodeMappingModel)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSourceSystem">Mapping source system, constant value from class VaccineCodeMappingModel.SourceSystemClass</param>
        ''' <param name="strTargetSystem">Mapping target system, constant value from class VaccineCodeMappingModel.TargetSystemClass</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String) As List(Of VaccineCodeMappingModel)
            Return GetBySystem(strSourceSystem, strTargetSystem, False)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSourceSystem">Mapping source system, constant value from class VaccineCodeMappingModel.SourceSystemClass</param>
        ''' <param name="strTargetSystem">Mapping target system, constant value from class VaccineCodeMappingModel.TargetSystemClass</param>
        ''' <param name="strCode">Vaccine code from source system</param>
        ''' <returns>Vaccine code mapping model for retrieve target vaccine code and display name</returns>
        ''' <remarks></remarks>
        Public Function GetMappingByCode(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strCode As String, ByVal strBrandID As String) As VaccineCodeMappingModel
            Return CType(MyBase.Item(GenerateKey(strSourceSystem, strTargetSystem, strCode, strBrandID)), VaccineCodeMappingModel)
        End Function

#Region "Private Function"

        Private Sub AddBySystem(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel)
            Dim strKey As String = GenerateKeyBySystem(udtVaccineCodeMappingModel)

            Dim listSystem As List(Of VaccineCodeMappingModel) = GetBySystem(udtVaccineCodeMappingModel, True)

            If Not listSystem.Contains(udtVaccineCodeMappingModel) Then
                listSystem.Add(udtVaccineCodeMappingModel)
            End If
        End Sub

        Private Sub RemoveBySystem(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel)
            Dim strKey As String = GenerateKeyBySystem(udtVaccineCodeMappingModel)

            Dim listSystem As List(Of VaccineCodeMappingModel) = GetBySystem(udtVaccineCodeMappingModel, False)

            If listSystem Is Nothing Then Exit Sub

            If listSystem.Contains(udtVaccineCodeMappingModel) Then
                listSystem.Remove(udtVaccineCodeMappingModel)
            End If
        End Sub

        Private Function GetBySystem(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel, ByVal bCreateIfNotExist As Boolean)
            Return GetBySystem(udtVaccineCodeMappingModel.SourceSystem, udtVaccineCodeMappingModel.TargetSystem, bCreateIfNotExist)
        End Function

        Private Function GetBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal bCreateIfNotExist As Boolean)
            Dim strKey As String = GenerateKeyBySystem(strSourceSystem, strTargetSystem)

            Dim listSystem As List(Of VaccineCodeMappingModel) = Nothing

            If _listSystem.Contains(strKey) Then
                listSystem = _listSystem(strKey)
            Else
                If bCreateIfNotExist Then
                    listSystem = New List(Of VaccineCodeMappingModel)
                    _listSystem.Add(strKey, listSystem)
                End If
            End If

            Return listSystem
        End Function

        Private Function GenerateKeyBySystem(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel) As String
            Return GenerateKeyBySystem(udtVaccineCodeMappingModel.SourceSystem, _
                                udtVaccineCodeMappingModel.TargetSystem)
        End Function

        Private Function GenerateKeyBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String) As String
            Return strSourceSystem + "|" + strTargetSystem
        End Function

        Private Function GenerateKey(ByVal udtVaccineCodeMappingModel As VaccineCodeMappingModel) As String
            Return GenerateKey(udtVaccineCodeMappingModel.SourceSystem, _
                                udtVaccineCodeMappingModel.TargetSystem, _
                                udtVaccineCodeMappingModel.VaccineCodeSource, _
                                udtVaccineCodeMappingModel.VaccineBrandIDSource)
        End Function

        Private Function GenerateKey(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strCode As String, ByVal strBrandID As String) As String
            Return strSourceSystem + "|" + strTargetSystem + "|" + strCode + "|" + strBrandID
        End Function

#End Region
    End Class
End Namespace

