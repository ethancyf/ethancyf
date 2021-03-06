Imports Common.Component.StaticData

Namespace Component.StaticData
    <Serializable()> Public Class StaticDataModelCollection
        'Inherits System.Collections.SortedList
        Inherits System.Collections.ArrayList

        Public Overloads Sub add(ByVal udtStaticDataModel As StaticDataModel)
            'MyBase.Add(udtStaticDataModel.ColumnName + "-" + udtStaticDataModel.ItemNo, udtStaticDataModel)
            MyBase.Add(udtStaticDataModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strColumnName As String, ByVal strItemNo As String) As StaticDataModel
            'Get
            '    Return CType(MyBase.Item(strColumnName + "-" + strItemNo), StaticDataModel)
            'End Get
            Get
                Dim intIdx As Integer
                Dim udtStaticData As StaticDataModel

                For intIdx = 0 To MyBase.Count - 1
                    udtStaticData = CType(MyBase.Item(intIdx), StaticDataModel)
                    If udtStaticData.ColumnName = strColumnName And udtStaticData.ItemNo = strItemNo Then
                        Return udtStaticData
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub remove(ByVal udtStaticDataModel As StaticDataModel)
            MyBase.Remove(udtStaticDataModel)
        End Sub

        Public Function Filter(ByVal strColumnName As String) As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection
            Dim udtStaticDataModel As StaticDataModel
            For Each udtStaticDataModel In Me
                If udtStaticDataModel.ColumnName = strColumnName Then
                    'udtStaticDataModelCollection.add(udtStaticDataModel.ColumnName + "-" + udtStaticDataModel.ItemNo, udtStaticDataModel)
                    udtStaticDataModelCollection.add(udtStaticDataModel)
                End If
            Next

            Return udtStaticDataModelCollection
        End Function


        'CRE20-009 decalre a function for filtering the ddlDocumentaryProof [Start][Nichole]
        Public Function CSSAFilter(ByVal strColumnName As String, ByVal strServiceDate As String) As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection
            Dim udtStaticDataModel As StaticDataModel


            For Each udtStaticDataModel In Me
                If udtStaticDataModel.ColumnName = strColumnName Then
                    
                    If udtStaticDataModel.ServiceDate = "" Then
                        udtStaticDataModelCollection.add(udtStaticDataModel)
                    Else
                        'If Date.TryParse(strServiceDate, rtnDate) >= Date.TryParse(udtStaticDataModel.ServiceDate, rtnDate) Then
                        If DateTime.Compare(strServiceDate, udtStaticDataModel.ServiceDate) > 0 Then
                            udtStaticDataModelCollection.add(udtStaticDataModel)
                        End If
                        'End If
                    End If
                End If
            Next

            Return udtStaticDataModelCollection
        End Function
        'CRE20-009 decalre a function for filtering the ddlDocumentaryProof [End][Nichole]
    End Class
End Namespace

