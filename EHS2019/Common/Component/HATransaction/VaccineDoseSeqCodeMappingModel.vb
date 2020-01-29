Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.HATransaction
    <Serializable()> _
    Public Class VaccineDoseSeqCodeMappingModel

#Region "Class"
        Public Class SourceSystemClass
            Public Const EHS As String = "EHS"
            Public Const CMS As String = "CMS"
            Public Const CIMS As String = "CIMS"
        End Class

        Public Class TargetSystemClass
            Public Const EHS As String = "EHS"
            Public Const CMS As String = "CMS"
            Public Const CIMS As String = "CIMS"
        End Class
#End Region

        Private _strSourceSystem As String
        Public ReadOnly Property SourceSystem() As String
            Get
                Return _strSourceSystem
            End Get
        End Property

        Private _strTargetSystem As String
        Public ReadOnly Property TargetSystem() As String
            Get
                Return _strTargetSystem
            End Get
        End Property

        Private _strVaccineDoseSeqCodeSource As String
        Public ReadOnly Property VaccineDoseSeqCodeSource() As String
            Get
                Return _strVaccineDoseSeqCodeSource
            End Get
        End Property

        Private _strVaccineDoseSeqCodeTarget As String
        Public ReadOnly Property VaccineDoseSeqCodeTarget() As String
            Get
                Return _strVaccineDoseSeqCodeTarget
            End Get
        End Property

        Private _strVaccineDoseSeqCodeCommon As String
        Public ReadOnly Property VaccineDoseSeqCodeCommon() As String
            Get
                Return _strVaccineDoseSeqCodeCommon
            End Get
        End Property

        Private _strVaccineDoseSeqCodeDesc As String
        Public ReadOnly Property VaccineDoseSeqCodeDesc() As String
            Get
                Return _strVaccineDoseSeqCodeDesc
            End Get
        End Property

        Private _strVaccineDoseSeqCodeDescChinese As String
        Public ReadOnly Property VaccineDoseSeqCodeDescChinese() As String
            Get
                Return _strVaccineDoseSeqCodeDescChinese
            End Get
        End Property

        Private _strDisplaySourceVaccineDoseDesc As String
        Public ReadOnly Property DisplaySourceVaccineDoseDesc() As Boolean
            Get
                Return IIf(_strDisplaySourceVaccineDoseDesc = YesNo.Yes, True, False)
            End Get
        End Property

        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strSubsidizeItemCodeSource As String
        Public ReadOnly Property SubsidizeItemCodeSource() As String
            Get
                Return _strSubsidizeItemCodeSource
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strVaccineDoseSeqCodeSource As String, ByVal strSubsidizeItemCodeSource As String, _
                        ByVal strVaccineDoseSeqCodeTarget As String, ByVal strVaccineDoseSeqCodeCommon As String, _
                        ByVal strVaccineDoseSeqCodeDesc As String, ByVal strVaccineDoseSeqCodeDescChinese As String, _
                        ByVal strDisplaySourceVaccineDoseDesc As String)
            'Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
            '                ByVal strVaccineDoseSeqCodeSource As String, ByVal strVaccineDoseSeqCodeTarget As String, ByVal strVaccineDoseSeqCodeCommon As String, _
            '                ByVal strVaccineDoseSeqCodeDesc As String, ByVal strVaccineDoseSeqCodeDescChinese As String, _
            '                ByVal strDisplaySourceVaccineDoseDesc As String)
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
            _strSourceSystem = strSourceSystem
            _strTargetSystem = strTargetSystem
            _strVaccineDoseSeqCodeSource = strVaccineDoseSeqCodeSource
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            _strSubsidizeItemCodeSource = strSubsidizeItemCodeSource
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
            _strVaccineDoseSeqCodeTarget = strVaccineDoseSeqCodeTarget
            _strVaccineDoseSeqCodeCommon = strVaccineDoseSeqCodeCommon
            _strVaccineDoseSeqCodeDesc = strVaccineDoseSeqCodeDesc
            _strVaccineDoseSeqCodeDescChinese = strVaccineDoseSeqCodeDescChinese
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            _strDisplaySourceVaccineDoseDesc = strDisplaySourceVaccineDoseDesc
            ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]
        End Sub
    End Class
End Namespace