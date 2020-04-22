Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Component.DHTransaction
    <Serializable()> Public Class HKMTTVaccineMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        ''' <summary>
        ''' A list store sub list by source and target system
        ''' </summary>
        ''' <remarks></remarks>
        Private _listSystem As New ListDictionary()

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel)
            MyBase.Add(udtHKMTTVaccineMappingModel.GenerateKey(), udtHKMTTVaccineMappingModel)

            AddBySystem(udtHKMTTVaccineMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel)
            MyBase.Remove(udtHKMTTVaccineMappingModel.GenerateKey)

            RemoveBySystem(udtHKMTTVaccineMappingModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As HKMTTVaccineMappingModel
            Get
                Return CType(MyBase.Item(intIndex), HKMTTVaccineMappingModel)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSourceSystem">Mapping source system, constant value from class VaccineCodeMappingModel.SourceSystemClass</param>
        ''' <param name="strTargetSystem">Mapping target system, constant value from class VaccineCodeMappingModel.TargetSystemClass</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String) As ListDictionary
            Return GetBySystem(strSourceSystem, strTargetSystem, False)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="udtDHVaccineModel">DH vaccine information</param>
        ''' <returns>Vaccine code mapping model for retrieving target vaccine code in eHS(S)</returns>
        ''' <remarks></remarks>
        Public Function GetMapping(ByVal udtDHVaccineModel As DHVaccineModel) As HKMTTVaccineMappingModel
            'Dim udtHKMTTVaccineMapping As HKMTTVaccineMappingModel = CType(MyBase.Item(HKMTTVaccineMappingModel.GenerateKey(udtDHVaccineModel)), HKMTTVaccineMappingModel)
            Dim lstVaccine As ListDictionary = GetBySystem(HKMTTVaccineMappingModel.SourceSystemClass.CIMS, HKMTTVaccineMappingModel.TargetSystemClass.EHS, False)

            Dim strVaccineMappingKey As String = HKMTTVaccineMappingModel.GenerateKey(udtDHVaccineModel)

            If lstVaccine.Contains(strVaccineMappingKey) Then
                Return lstVaccine(strVaccineMappingKey)
            Else
                ' Store undefined HKMTT vaccine for monitoring
                DHVaccineBLL.DHVaccineDAL.AddHKMTTVaccineMappingUndefined(udtDHVaccineModel)
                Return lstVaccine(HKMTTVaccineMappingModel.GenerateKeyUnknownVaccine(udtDHVaccineModel))
            End If

        End Function

#Region "Private Function"

        Private Sub AddBySystem(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel)
            Dim strKey As String = udtHKMTTVaccineMappingModel.GenerateKeyBySystem()

            Dim listVaccine As ListDictionary = GetBySystem(udtHKMTTVaccineMappingModel, True)
            Dim strVaccineMappingKey As String = udtHKMTTVaccineMappingModel.GenerateKey()
            If Not listVaccine.Contains(strVaccineMappingKey) Then
                listVaccine.Add(strVaccineMappingKey, udtHKMTTVaccineMappingModel)
            End If
        End Sub

        Private Sub RemoveBySystem(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel)
            Dim strKey As String = udtHKMTTVaccineMappingModel.GenerateKeyBySystem()
            Dim strVaccineMappingKey As String = udtHKMTTVaccineMappingModel.GenerateKey()

            Dim listVaccine As ListDictionary = GetBySystem(udtHKMTTVaccineMappingModel, False)

            If listVaccine Is Nothing Then Exit Sub

            If Not listVaccine.Contains(strVaccineMappingKey) Then
                listVaccine.Remove(strVaccineMappingKey)
            End If
        End Sub

        Private Function GetBySystem(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel, ByVal bCreateIfNotExist As Boolean) As ListDictionary
            Return GetBySystem(udtHKMTTVaccineMappingModel.SourceSystem, udtHKMTTVaccineMappingModel.TargetSystem, bCreateIfNotExist)
        End Function

        Private Function GetBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal bCreateIfNotExist As Boolean) As ListDictionary
            Dim strKey As String = HKMTTVaccineMappingModel.GenerateKeyBySystem(strSourceSystem, strTargetSystem)

            Dim listVaccine As ListDictionary = Nothing

            If _listSystem.Contains(strKey) Then
                listVaccine = _listSystem(strKey)
            Else
                If bCreateIfNotExist Then
                    listVaccine = New ListDictionary()
                    _listSystem.Add(strKey, listVaccine)
                End If
            End If

            Return listVaccine
        End Function


#End Region
    End Class
End Namespace

