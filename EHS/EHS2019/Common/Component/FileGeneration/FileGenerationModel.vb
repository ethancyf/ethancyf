Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationModel

#Region "Schema"
        'File_ID	varchar(30)	Unchecked
        'File_Name	varchar(100)	Checked
        'File_Desc	varchar(500)	Checked
        'File_Name_Prefix	varchar(10)	Checked
        'File_Type	varchar(50)	Checked
        'File_Data_SP	varchar(100)	Checked
        'FilterCriteria_UC	varchar(100)	Checked
        'Report_Template	varchar(100)	Checked
        'Group_ID	varchar(30)	Checked
        'Is_SelfAccess	char(1)	Checked
        'Auto_Generate	char(1)	Checked
        'Show_for_Generation	char(1)	Checked
        'Frequency	char(1)	Checked
        'Day_of_Generation	int	Checked
        'Message_Subject	nvarchar(300)	Checked
        'Message_Content	ntext	Checked
        'Create_Dtm	datetime	Checked
        'Create_By	varchar(20)	Checked
        'Update_Dtm	datetime	Checked
        'Update_By	varchar(20)	Checked

#End Region

#Region "Private Member"

        Private _strFile_ID As String
        Private _strFile_Name As String
        Private _strFile_Desc As String
        Private _strFile_Name_Prefix As String
        Private _strFile_Type As String
        ' CRE15-006 Rename of eHS [Start][Lawrence]
        Private _strDisplay_Code As String
        Private _strOutput_File_Name As String
        Private _strOutput_File_Description As String
        Private _strFile_Prepare_Data_SP As String
        ' CRE15-006 Rename of eHS [End][Lawrence]
        Private _strFile_Data_SP As String

        Private _strFilterCriteria_UC As String
        Private _strFilterCriteria_UC_Wording As String

        Private _strReport_Template As String
        Private _strGroup_ID As String
        Private _strIs_SelfAccess As String
        Private _strAuto_Generate As String

        Private _strShow_for_Generation As String
        Private _strFrequency As String

        Private _intDay_of_Generation As Integer

        Private _strMessage_Subject As String
        Private _strMessage_Content As String

        Private _dtmCreate_Dtm As Nullable(Of DateTime)
        Private _strCreate_By As String
        Private _dtmUpdate_Dtm As Nullable(Of DateTime)
        Private _strUpdate_By As String

        Private _strGet_Data_From_Bak As String

        Private _listOfXLS_Parameter As List(Of Integer)

        Private _udtFileGenerationUserControlList As FileGenerationUserControlModelCollection

#End Region

#Region "SQL DataType"

        Public Const File_IDDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_IDDataSize As Integer = 30

        Public Const File_NameDataType As SqlDbType = SqlDbType.VarChar
        ' CRE16-014 to 016 (Voucher aberrant and new monitoring) [Start][Koala]
        Public Const File_NameDataSize As Integer = 200
        'Public Const File_NameDataSize As Integer = 100
        ' CRE16-014 to 016 (Voucher aberrant and new monitoring) [End][Koala]

        Public Const File_DescDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_DescDataSize As Integer = 500

        Public Const File_Name_PrefixDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_Name_PrefixDataSize As Integer = 10

        Public Const File_TypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_TypeDataSize As Integer = 50

        Public Const File_Data_SPDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_Data_SPDataSize As Integer = 100

        Public Const FilterCriteria_UCType As SqlDbType = SqlDbType.VarChar
        Public Const FilterCriteria_UCSize As Integer = 100

        Public Const Report_TemplateDataType As SqlDbType = SqlDbType.VarChar
        Public Const Report_TemplateDataSize As Integer = 100

        Public Const Group_IDDataType As SqlDbType = SqlDbType.VarChar
        Public Const Group_IDDataSize As Integer = 30

        Public Const Is_SelfAccessDataType As SqlDbType = SqlDbType.Char
        Public Const Is_SelfAccessDataSize As Integer = 1

        Public Const Auto_GenerateDataType As SqlDbType = SqlDbType.Char
        Public Const Auto_GenerateDataSize As Integer = 1

        Public Const Show_for_GenerationDataType As SqlDbType = SqlDbType.Char
        Public Const Show_for_GenerationDataSize As Integer = 1


        Public Const FrequencyDataType As SqlDbType = SqlDbType.Char
        Public Const FrequencyDataSize As Integer = 1


        Public Const Day_of_GenerationDataType As SqlDbType = SqlDbType.Int
        Public Const Day_of_GenerationDataSize As Integer = 4

        Public Const Message_SubjectDataType As SqlDbType = SqlDbType.NVarChar
        ' CRE16-014 to 016 (Voucher aberrant and new monitoring) [Start][Koala]
        Public Const Message_SubjectDataSize As Integer = 800
        'Public Const Message_SubjectDataSize As Integer = 300
        ' CRE16-014 to 016 (Voucher aberrant and new monitoring) [End][Koala]

        Public Const Message_ContentDataType As SqlDbType = SqlDbType.NText
        Public Const Message_ContentDataSize As Integer = 2147483647

        Public Const Create_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_DtmDataSize As Integer = 8

        Public Const Create_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_ByDataSize As Integer = 20

        Public Const Update_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_DtmDataSize As Integer = 8

        Public Const Update_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_ByDataSize As Integer = 20

#End Region

#Region "Property"

        Public Property FileID() As String
            Get
                Return Me._strFile_ID
            End Get
            Set(ByVal value As String)
                Me._strFile_ID = value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return Me._strFile_Name
            End Get
            Set(ByVal value As String)
                Me._strFile_Name = value
            End Set
        End Property

        Public Property FileDesc() As String
            Get
                Return Me._strFile_Desc
            End Get
            Set(ByVal value As String)
                Me._strFile_Desc = value
            End Set
        End Property

        Public Property FileNamePrefix() As String
            Get
                Return Me._strFile_Name_Prefix
            End Get
            Set(ByVal value As String)
                Me._strFile_Name_Prefix = value
            End Set
        End Property

        Public Property FileType() As String
            Get
                Return Me._strFile_Type
            End Get
            Set(ByVal value As String)
                Me._strFile_Type = value
            End Set
        End Property

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        Public Property DisplayCode() As String
            Get
                Return Me._strDisplay_Code
            End Get
            Set(ByVal value As String)
                Me._strDisplay_Code = value
            End Set
        End Property

        Public Property OutputFileName() As String
            Get
                Return Me._strOutput_File_Name
            End Get
            Set(ByVal value As String)
                Me._strOutput_File_Name = value
            End Set
        End Property

        Public ReadOnly Property OutputFileName(ByVal dtm As DateTime, Optional dicReplace As Dictionary(Of String, String) = Nothing) As String
            Get
                Dim s As String = _strOutput_File_Name

                ' Basic replacement
                s = s.Replace("[%YYYYMMDD%]", dtm.ToString("yyyyMMdd")) _
                        .Replace("[%HHMMSS%]", dtm.ToString("HHmmss")) _
                        .Replace("[%HHMM%]", dtm.ToString("HHmm")) _
                        .Replace("[%YYYY%]", dtm.ToString("yyyy")) _
                        .Replace("[%MMM%]", dtm.ToString("MMM")) _
                        .Replace("[%DD%]", dtm.ToString("dd")) _
                        .Replace("[%FFF%]", dtm.ToString("fff")) _
                        .Replace("[%FileType%]", (New Common.Format.Formatter).FormatFileExt(Me.FileType))

                ' Extra replacement
                If Not IsNothing(dicReplace) Then
                    For Each kvp As KeyValuePair(Of String, String) In dicReplace
                        s = s.Replace(String.Format("[%{0}%]", kvp.Key), kvp.Value)
                    Next
                End If

                ' Return
                Return s

            End Get
        End Property

        Public Property OutputFileDescription() As String
            Get
                Return Me._strOutput_File_Description
            End Get
            Set(ByVal value As String)
                Me._strOutput_File_Description = value
            End Set
        End Property

        Public ReadOnly Property OutputFileDescription(ByVal strOutputFileName As String) As String
            Get
                Return _strOutput_File_Description.Replace("[%FileName%]", strOutputFileName)
            End Get
        End Property

        Public Property FilePrepareDataSP() As String
            Get
                Return Me._strFile_Prepare_Data_SP
            End Get
            Set(ByVal value As String)
                Me._strFile_Prepare_Data_SP = value
            End Set
        End Property
        ' CRE15-006 Rename of eHS [End][Lawrence]

        Public Property FileDataSP() As String
            Get
                Return Me._strFile_Data_SP
            End Get
            Set(ByVal value As String)
                Me._strFile_Data_SP = value
            End Set
        End Property

        Public Property FilterCriteriaUC() As String
            Get
                Return Me._strFilterCriteria_UC
            End Get
            Set(ByVal value As String)
                Me._strFilterCriteria_UC = value
            End Set
        End Property

        Public Property FilterCriteriaUCWording() As String
            Get
                Return Me._strFilterCriteria_UC_Wording
            End Get
            Set(ByVal value As String)
                Me._strFilterCriteria_UC_Wording = value
            End Set
        End Property

        Public Property ReportTemplate() As String
            Get
                Return Me._strReport_Template
            End Get
            Set(ByVal value As String)
                Me._strReport_Template = value
            End Set
        End Property

        Public Property GroupID() As String
            Get
                Return Me._strGroup_ID
            End Get
            Set(ByVal value As String)
                Me._strGroup_ID = value
            End Set
        End Property

        Public Property IsSelfAccess() As String
            Get
                Return Me._strIs_SelfAccess
            End Get
            Set(ByVal value As String)
                Me._strIs_SelfAccess = value
            End Set
        End Property

        Public Property AutoGenerate() As String
            Get
                Return Me._strAuto_Generate
            End Get
            Set(ByVal value As String)
                Me._strAuto_Generate = value
            End Set
        End Property

        Public Property ShowForGeneration() As String
            Get
                Return Me._strShow_for_Generation
            End Get
            Set(ByVal value As String)
                Me._strShow_for_Generation = value
            End Set
        End Property

        Public Property DayofGeneration() As Integer
            Get
                Return Me._intDay_of_Generation
            End Get
            Set(ByVal value As Integer)
                Me._intDay_of_Generation = value
            End Set
        End Property

        Public Property MessageSubject() As String
            Get
                Return Me._strMessage_Subject
            End Get
            Set(ByVal value As String)
                Me._strMessage_Subject = value
            End Set
        End Property

        Public Property MessageContent() As String
            Get
                Return Me._strMessage_Content
            End Get
            Set(ByVal value As String)
                Me._strMessage_Content = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCreate_Dtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property


        Public Property GetDataFromBak() As String
            Get
                Return Me._strGet_Data_From_Bak
            End Get
            Set(ByVal value As String)
                Me._strGet_Data_From_Bak = value
            End Set
        End Property

        Public Property XLS_Parameter() As List(Of Integer)
            Get
                Return Me._listOfXLS_Parameter
            End Get
            Set(ByVal value As List(Of Integer))
                Me._listOfXLS_Parameter = value
            End Set
        End Property

        Public Property UserControlList() As FileGenerationUserControlModelCollection
            Get
                Return _udtFileGenerationUserControlList
            End Get
            Set(ByVal value As FileGenerationUserControlModelCollection)
                _udtFileGenerationUserControlList = value
            End Set
        End Property

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        Public ReadOnly Property FileNameWithDateTimeStamp()
            Get
                Return FileNamePrefix + Now.ToString("yyyyMMdd_HHmmss") + "." + FileType.ToLower
            End Get
        End Property

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtFileGenerationModel As FileGenerationModel)

            Me._strFile_ID = udtFileGenerationModel._strFile_ID
            Me._strFile_Name = udtFileGenerationModel._strFile_Name
            Me._strFile_Desc = udtFileGenerationModel._strFile_Desc
            Me._strFile_Name_Prefix = udtFileGenerationModel._strFile_Name_Prefix
            Me._strFile_Type = udtFileGenerationModel._strFile_Type
            Me._strDisplay_Code = udtFileGenerationModel._strDisplay_Code
            Me._strOutput_File_Name = udtFileGenerationModel._strOutput_File_Name
            Me._strOutput_File_Description = udtFileGenerationModel._strOutput_File_Description
            Me._strFile_Prepare_Data_SP = udtFileGenerationModel._strFile_Prepare_Data_SP
            Me._strFile_Data_SP = udtFileGenerationModel._strFile_Data_SP

            Me._strFilterCriteria_UC = udtFileGenerationModel._strFilterCriteria_UC
            Me._strReport_Template = udtFileGenerationModel._strReport_Template
            Me._strGroup_ID = udtFileGenerationModel._strGroup_ID
            Me._strIs_SelfAccess = udtFileGenerationModel._strIs_SelfAccess
            Me._strAuto_Generate = udtFileGenerationModel._strAuto_Generate

            Me._strShow_for_Generation = udtFileGenerationModel._strShow_for_Generation
            Me._strFrequency = udtFileGenerationModel._strFrequency

            Me._intDay_of_Generation = udtFileGenerationModel._intDay_of_Generation

            Me._strMessage_Subject = udtFileGenerationModel._strMessage_Subject
            Me._strMessage_Content = udtFileGenerationModel._strMessage_Content

            Me._dtmCreate_Dtm = udtFileGenerationModel._dtmCreate_Dtm
            Me._strCreate_By = udtFileGenerationModel._strCreate_By
            Me._dtmUpdate_Dtm = udtFileGenerationModel._dtmUpdate_Dtm
            Me._strUpdate_By = udtFileGenerationModel._strUpdate_By

            Me._strGet_Data_From_Bak = udtFileGenerationModel._strGet_Data_From_Bak
            Me._listOfXLS_Parameter = udtFileGenerationModel.XLS_Parameter

        End Sub

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        Public Sub New( _
            ByVal strFile_ID As String, _
            ByVal strFile_Name As String, _
            ByVal strFile_Desc As String, _
            ByVal strFile_Name_Prefix As String, _
            ByVal strFile_Type As String, _
            ByVal strDisplay_Code As String, _
            ByVal strOutput_File_Name As String, _
            ByVal strOutput_File_Description As String, _
            ByVal strFile_Prepare_Data_SP As String, _
            ByVal strFile_Data_SP As String, _
            ByVal strFilterCriteria_UC As String, _
            ByVal strReport_Template As String, _
            ByVal strGroup_ID As String, _
            ByVal strIs_SelfAccess As String, _
            ByVal strAuto_Generate As String, _
            ByVal strShow_for_Generation As String, _
            ByVal strFrequency As String, _
            ByVal intDay_of_Generation As Integer, _
            ByVal strMessage_Subject As String, _
            ByVal strMessage_Content As String, _
            ByVal dtmCreate_Dtm As Nullable(Of DateTime), _
            ByVal strCreate_By As String, _
            ByVal dtmUpdate_Dtm As Nullable(Of DateTime), _
            ByVal strUpdate_By As String, _
            ByVal strGet_Data_From_Bak As String, _
            ByVal listOfXLS_Parameter As List(Of Integer))

            Me._strFile_ID = strFile_ID
            Me._strFile_Name = strFile_Name
            Me._strFile_Desc = strFile_Desc
            Me._strFile_Name_Prefix = strFile_Name_Prefix
            Me._strFile_Type = strFile_Type
            Me._strDisplay_Code = strDisplay_Code
            Me._strOutput_File_Name = strOutput_File_Name
            Me._strOutput_File_Description = strOutput_File_Description
            Me._strFile_Prepare_Data_SP = strFile_Prepare_Data_SP
            Me._strFile_Data_SP = strFile_Data_SP

            Me._strFilterCriteria_UC = strFilterCriteria_UC
            Me._strReport_Template = strReport_Template
            Me._strGroup_ID = strGroup_ID
            Me._strIs_SelfAccess = strIs_SelfAccess
            Me._strAuto_Generate = strAuto_Generate

            Me._strShow_for_Generation = strShow_for_Generation
            Me._strFrequency = strFrequency

            Me._intDay_of_Generation = intDay_of_Generation

            Me._strMessage_Subject = strMessage_Subject
            Me._strMessage_Content = strMessage_Content

            Me._dtmCreate_Dtm = dtmCreate_Dtm
            Me._strCreate_By = strCreate_By
            Me._dtmUpdate_Dtm = dtmUpdate_Dtm
            Me._strUpdate_By = strUpdate_By

            Me._strGet_Data_From_Bak = strGet_Data_From_Bak

            Me._listOfXLS_Parameter = listOfXLS_Parameter

        End Sub

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

#End Region

    End Class
End Namespace