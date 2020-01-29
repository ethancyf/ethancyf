Namespace Component.ThirdParty
    <Serializable()> Public Class ThirdPartyEnrollRecordModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub Add(ByVal udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel)
            MyBase.Add(udtThirdPartyEnrollRecordModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel)
            MyBase.Remove(udtThirdPartyEnrollRecordModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal index As Integer) As ThirdPartyEnrollRecordModel
            Get
                Return DirectCast(MyBase.Item(index), ThirdPartyEnrollRecordModel)
            End Get
        End Property

        'Default Public Overloads ReadOnly Property Item(ByVal strSysCode As String, ByVal strEnrolmentRefNo As String) As ThirdPartyEnrollRecordModel
        '    Get
        '        Return CType(MyBase.Item(GenerateKey(strSysCode, strEnrolmentRefNo)), ThirdPartyEnrollRecordModel)
        '    End Get
        'End Property

        Public Sub New()

        End Sub

    End Class
End Namespace

