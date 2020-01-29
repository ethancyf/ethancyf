Namespace Component.eHealthAccountDeathRecord
    <Serializable()> Partial Public Class DeathRecordEntryModel

#Region "Comment"


#End Region

#Region "Schema"
        'Table <DeathRecordEntry>

        'Encrypt_Field1	        varbinary	100
        'Death_Record_File_ID	char	    15
        'DOD	                datetime	8
        'Exact_DOD	            char	    1
        'DOR	                datetime	8
        'Encrypt_Field2	        varbinary	100
        'Record_Status	        char	    1
#End Region

#Region "Memeber"

        Private _strVoucher_Acc_ID As String
        Private _strScheme_Code As String
        Private _strRecord_Status As String
        Private _strRemark As String
        Private _strPublic_Enquiry_Status As String

        Private _strPublic_Enq_Status_Remark As String
        Private _dtmEffective_Dtm As Nullable(Of DateTime)
        Private _dtmTerminate_Dtm As Nullable(Of DateTime)
        Private _dtmCreate_Dtm As DateTime
        Private _strCreate_By As String

        Private _dtmUpdate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _strDataEntry_By As String
        Private _byteTSMP As Byte()


#End Region

#Region "Property"

        Private _strDocNo As String
        Public ReadOnly Property DocNo() As String
            Get
                Return _strDocNo
            End Get
        End Property

        Private _strDeathRecordFileID As String
        Public ReadOnly Property DeathRecordFileID() As String
            Get
                Return _strDeathRecordFileID
            End Get
        End Property

        Private _dtmDOD As DateTime
        Public ReadOnly Property DOD() As DateTime
            Get
                Return _dtmDOD
            End Get
        End Property

        Private _strExactDOD As String
        Public ReadOnly Property ExactDOD() As String
            Get
                Return _strExactDOD
            End Get
        End Property

        Private _dtmDOR As DateTime
        Public ReadOnly Property DOR() As DateTime
            Get
                Return _dtmDOR
            End Get
        End Property

        Private _strEnglishName As String
        Public ReadOnly Property EnglishName() As String
            Get
                Return _strEnglishName
            End Get
        End Property

        Private _strRecordStatus As String
        Public ReadOnly Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
        End Property

        Private _blnIsDead As Boolean = True
        Public ReadOnly Property IsDead() As Boolean
            Get
                Return _blnIsDead
            End Get
        End Property

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' This functions will be replaced by [EHSPersonalInformationModel]
        'Public ReadOnly Property IsDead(ByVal dtmServiceDate As DateTime) As Boolean
        '    Get
        '        If Not IsDead() Then Return False

        '        Select Case _strExactDOD
        '            Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.D
        '                Return DateDiff(DateInterval.Day, DOD, dtmServiceDate) > 0
        '            Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.M
        '                Return DateDiff(DateInterval.Month, DOD, dtmServiceDate) > 0
        '            Case eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.Y
        '                Return DateDiff(DateInterval.Year, DOD, dtmServiceDate) > 0
        '        End Select
        '    End Get
        'End Property
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public ReadOnly Property FormattedDOD() As String
            Get
                Return (New Common.Format.Formatter).formatDOB(Me.DOD, Me.ExactDOD.Trim, String.Empty, Nothing, Nothing)
            End Get
        End Property

        Public ReadOnly Property FormattedDOR() As String
            Get
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Return (New Common.Format.Formatter).formatDate(Me.DOR, String.Empty)
                Return (New Common.Format.Formatter).formatDisplayDate(Me.DOR, String.Empty)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End Get

        End Property
        Public ReadOnly Property FormattedDocNo(ByVal blnMasked As Boolean) As String
            Get
                Return (New Common.Format.Formatter).formatHKID(Me.DocNo, blnMasked)
            End Get
        End Property

#End Region

#Region "Constructor"

        ''' <summary>
        ''' Default constructor to create a empty death record
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _blnIsDead = False
        End Sub

        ''' <summary>
        ''' Constructor for Dealth Record Entry from database
        ''' </summary>
        ''' <param name="strDocNo"></param>
        ''' <param name="strDeathRecordFileID"></param>
        ''' <param name="dtmDOD"></param>
        ''' <param name="strExactDOD"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="strEnglishName"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strDocNo As String, _
                        ByVal strDeathRecordFileID As String, _
                        ByVal dtmDOD As DateTime, _
                        ByVal strExactDOD As String, _
                        ByVal dtmDOR As DateTime, _
                        ByVal strEnglishName As String, _
                        ByVal strRecordStatus As String)

            Init(strDocNo, _
                 strDeathRecordFileID, _
                 dtmDOD, _
                 strExactDOD, _
                 dtmDOR, _
                 strEnglishName, _
                 strRecordStatus)
        End Sub

        Public Sub New(ByVal drDeathRecordEntry As DataRow)
            Init(drDeathRecordEntry("Document_No"), _
                        drDeathRecordEntry("Death_Record_File_ID"), _
                        drDeathRecordEntry("DOD"), _
                        drDeathRecordEntry("Exact_DOD"), _
                        drDeathRecordEntry("DOR"), _
                        drDeathRecordEntry("Death_English_Name"), _
                        drDeathRecordEntry("Record_Status"))
        End Sub

        Private Sub Init(ByVal strDocNo As String, _
                        ByVal strDeathRecordFileID As String, _
                        ByVal dtmDOD As DateTime, _
                        ByVal strExactDOD As String, _
                        ByVal dtmDOR As DateTime, _
                        ByVal strEnglishName As String, _
                        ByVal strRecordStatus As String)

            _strDocNo = strDocNo
            _strDeathRecordFileID = strDeathRecordFileID
            _dtmDOD = dtmDOD
            _strExactDOD = strExactDOD
            _dtmDOR = dtmDOR
            _strEnglishName = strEnglishName
            _strRecordStatus = strRecordStatus
        End Sub
#End Region

    End Class
End Namespace
