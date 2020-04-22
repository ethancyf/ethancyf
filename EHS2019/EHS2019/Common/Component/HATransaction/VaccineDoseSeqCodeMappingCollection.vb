Imports Microsoft.VisualBasic

Namespace Component.HATransaction
    <Serializable()> Public Class VaccineDoseSeqCodeMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVaccineDoseSeqCodeMappingModel As VaccineDoseSeqCodeMappingModel)
            MyBase.Add(GenerateKey(udtVaccineDoseSeqCodeMappingModel), udtVaccineDoseSeqCodeMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtVaccineDoseSeqCodeMappingModel As VaccineDoseSeqCodeMappingModel)
            MyBase.Remove(GenerateKey(udtVaccineDoseSeqCodeMappingModel))
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VaccineDoseSeqCodeMappingModel
            Get
                Return CType(MyBase.Item(intIndex), VaccineDoseSeqCodeMappingModel)
            End Get
        End Property


        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSourceSystem">Mapping source system, constant value from class VaccineDoseSeqCodeMappingModel.SourceSystemClass</param>
        ''' <param name="strTargetSystem">Mapping target system, constant value from class VaccineDoseSeqCodeMappingModel.TargetSystemClass</param>
        ''' <param name="strCode">Dose sequence code from source system</param>
        ''' <param name="strItemCode">Subsidize Item code from source system</param>
        ''' <returns>Dose sequence mapping model for retrieve target dose sequence code and display name</returns>
        ''' <remarks>Filter by Specify Item Code first, if not found, filter by "ALL"</remarks>
        Public Function GetMappingByCode(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                                         ByVal strCode As String, ByVal strItemCode As String) As VaccineDoseSeqCodeMappingModel

            If strItemCode.Trim() = String.Empty Then
                strItemCode = "ALL"
            Else
                strItemCode = strItemCode.Trim()
            End If

            Dim udtDoseSeqCodeModel As VaccineDoseSeqCodeMappingModel = GetMappingByCode(GenerateKey(strSourceSystem, strTargetSystem, strCode, strItemCode))

            If udtDoseSeqCodeModel Is Nothing AndAlso strItemCode <> "ALL" Then
                strItemCode = "ALL"
                udtDoseSeqCodeModel = GetMappingByCode(GenerateKey(strSourceSystem, strTargetSystem, strCode, strItemCode))
            End If

            Return udtDoseSeqCodeModel
        End Function
        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

#Region "Private Function"

        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function GetMappingByCode(ByVal strKey As String) As VaccineDoseSeqCodeMappingModel
            Return CType(MyBase.Item(strKey), VaccineDoseSeqCodeMappingModel)
        End Function
        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

        Private Function GenerateKey(ByVal udtVaccineDoseSeqCodeMappingModel As VaccineDoseSeqCodeMappingModel) As String
            Return GenerateKey(udtVaccineDoseSeqCodeMappingModel.SourceSystem, _
                                udtVaccineDoseSeqCodeMappingModel.TargetSystem, _
                                udtVaccineDoseSeqCodeMappingModel.VaccineDoseSeqCodeSource, _
                                udtVaccineDoseSeqCodeMappingModel.SubsidizeItemCodeSource)
        End Function

        Private Function GenerateKey(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strCode As String, ByVal strItemCode As String) As String
            Return strSourceSystem + "|" + strTargetSystem + "|" + strCode + "|" + strItemCode
        End Function

#End Region

    End Class

End Namespace

