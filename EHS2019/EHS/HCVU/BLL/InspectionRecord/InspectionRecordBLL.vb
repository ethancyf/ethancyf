Imports Common.DataAccess
Imports Common.ComFunction
Imports System.Data.SqlClient
Imports Common.Component.HCVUUser
Imports Common.Component
Imports Common.Component.UserRole
Imports System.Globalization
Imports System.IO
Imports System.Xml
Imports Common.ComObject

Public Class InspectionRole
    Public IsObserver As Boolean = False
    Public IsOfficer As Boolean = False
    Public IsEndorser As Boolean = False
    Public IsSEO As Boolean = False
End Class

Public Class InspectionRecordBLL
    Dim db As New Database

    Private Const DefaultDateOutputFormat As String = "dd MMM yyyy"
    Private Const DefaultDateInputFormat As String = "dd-MM-yyyy"
    Private Const DefaultDateDBFormat As String = "dd/MM/yyyy"
#Region "Get Datatable from DB"
    ' Get Inspection Record List

    Public Function SearchInspectionRecordByAny(ByVal param As GetInspectionParameter) As DataTable
        Dim dt As New DataTable

        Dim prams = GenerateSearchInspectionRecordParameter(param)
        Try
            db.RunProc("proc_InspectionVisitInfo_get_byAny", prams, dt)
            Return dt.Copy
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw

        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try
    End Function
    Public Function SearchInspectionRecordByAny(ByVal strFunctionCode As String, ByVal param As GetInspectionParameter, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        Dim dt As New DataTable
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult = Nothing

        Dim prams = GenerateSearchInspectionRecordParameter(param)

        udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_InspectionVisitInfo_get_byAny", prams, blnOverrideResultLimit, db)
        Return udtBLLSearchResult
    End Function
    Private Function GenerateSearchInspectionRecordParameter(ByVal param As GetInspectionParameter)
        Dim prams() As SqlParameter = {
            db.MakeInParam("@Inspection_ID", InspectionRecordModel.InspectionIDDataType, InspectionRecordModel.InspectionIDDataSize,
                           param.InspectionID),
            db.MakeInParam("@File_Reference_No", SqlDbType.VarChar, 30,
                           param.FileReferenceNo),
            db.MakeInParam("@Referred_Reference_No", SqlDbType.VarChar, 30,
                           param.ReferredReferenceNo),
             db.MakeInParam("@Subject_Officer_ID", SqlDbType.VarChar, 20,
                           param.SubjectOfficerID),
            db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8,
                           param.SPID),
            db.MakeInParam("@Visit_Begin_Dtm", SqlDbType.DateTime, 8,
                           IIf(IsNothing(param.StartDtm), DBNull.Value, param.StartDtm)),
            db.MakeInParam("@Visit_End_Dtm", SqlDbType.DateTime, 8,
                           IIf(IsNothing(param.EndDtm), DBNull.Value, param.EndDtm)),
            db.MakeInParam("@Main_Type_Of_Inspection", SqlDbType.VarChar, 10,
                           param.MainTypeOfInspection),
            db.MakeInParam("@Record_Status", SqlDbType.VarChar, 10,
                           param.RecordStatus),
            db.MakeInParam("@Practice_Display_Seq", SqlDbType.Int, 10,
                           0),
            db.MakeInParam("@User_ID", SqlDbType.VarChar, 20,
                           param.UserId),
            db.MakeInParam("@OnlyOwner", SqlDbType.Int, 10,
                           param.OnlyForOwner)
        }
        Return prams
    End Function
    ' Get Inspection Record
    Public Function GetInspectionRecord(Inspection_ID As String) As InspectionRecordModel
        Dim dt As New DataTable

        Dim params() As SqlParameter = {
            db.MakeInParam("@Inspection_ID", SqlDbType.VarChar, 30,
                           Inspection_ID)
        }
        Try
            db.RunProc("proc_InspectionVisitInfo_get_byID", params, dt)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw

        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try

        If (dt.Rows.Count > 0) Then
            Return SerializeInspectionRecord(dt.Rows(0))
        Else
            Return Nothing
        End If
    End Function
    Public Function GetLatestRecordBySPID(ByVal strSPID As String, Optional excludeInspectionID As String = "") As InspectionRecordModel
        Dim dt As New DataTable

        Dim params() As SqlParameter = {
            db.MakeInParam("@SP_ID", InspectionRecordModel.SPIDDataType, InspectionRecordModel.SPIDDataSize,
                           strSPID),
            db.MakeInParam("@Filter_Date", SqlDbType.Date, 10,
                           DBNull.Value),
            db.MakeInParam("@Inspection_ID", SqlDbType.VarChar, 30,
                           excludeInspectionID)
        }
        Try
            db.RunProc("proc_InspectionVisitInfoLatest_get_bySPID", params, dt)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw

        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try

        If (dt.Rows.Count > 0) Then
            Return SerializeInspectionRecordLatest(dt.Rows(0))
        Else
            Return Nothing
        End If
    End Function
    ' Get Inspection Record For Print Out
    Public Function GetInspectionRecordByID_ForReport(Inspection_ID As String) As DataTable
        Dim dt As New DataTable
        Dim params() As SqlParameter = {
            db.MakeInParam("@Inspection_ID", InspectionRecordModel.InspectionIDDataType, InspectionRecordModel.InspectionIDDataSize,
                           Inspection_ID)
        }
        Try
            db.RunProc("proc_InspectionVisitInfo_get_byID_rpt", params, dt)
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw

        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try
        Return dt.Copy
    End Function
    ' Get User List by Role Type
    Public Function GetUserListByRoleType(ByVal arrRoleTypes As String()) As DataTable
        Dim db As New Database
        Dim dt As New DataTable
        Dim dtReturn As DataTable = Nothing
        Dim intRoleType As Integer
        Dim arrUserID As New List(Of String)
        Try
            For Each strRoleType As String In arrRoleTypes
                If Integer.TryParse(strRoleType, intRoleType) Then
                    Dim prams() As SqlParameter = {
                        db.MakeInParam("@intRoleType", SqlDbType.Int, 20,
                                       intRoleType)
                    }
                    db.RunProc("proc_HCVUUser_get_byRoleType", prams, dt)

                    If dtReturn Is Nothing Then
                        dtReturn = dt
                    Else
                        For Each row As DataRow In dt.Rows
                            dtReturn.Rows.Add(row)
                        Next
                        'dtReturn.Rows.
                    End If
                End If
            Next
        Catch eSQL As SqlException
            db.RollBackTranscation()
            Throw

        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try
        Return dtReturn
    End Function
#End Region
#Region "Get Datatable from Cache"
    ' Get Inspection Role
    Public Function GetInspectionRole(user As HCVUUserModel) As InspectionRole
        Dim role As InspectionRole = New InspectionRole
        For Each key As String In user.UserRoleCollection.Keys
            Dim userRole As UserRoleModel = CType(user.UserRoleCollection.Item(key), UserRoleModel)
            Select Case userRole.RoleType.ToString()
                Case RoleType.InspectionObserver
                    role.IsObserver = True
                Case RoleType.InspectionOfficer
                    role.IsOfficer = True
                Case RoleType.InspectionEndorser
                    role.IsEndorser = True
                Case RoleType.InspectionSEO
                    role.IsSEO = True
            End Select
        Next
        Return role
    End Function
    'Get Officer List
    Public Function GetInspectionOfficerList() As DataTable
        Dim arrRoleType() As String = New String(0) {RoleType.InspectionOfficer}
        Return GetUserListByRoleType(arrRoleType)
    End Function
    'Get All Practice by SP ID
    Public Function GetAllPractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As Practice.PracticeBLL.PracticeDisplayType) As DataTable
        Dim udtPracticeBLL As New Practice.PracticeBLL
        ' Get Practice Information
        Dim dtPractice As DataTable = udtPracticeBLL.getRawAllPracticeBankAcct(strSPID)
        Practice.PracticeBLL.ConcatePracticeDisplayColumn(dtPractice, Practice.PracticeBLL.PracticeDisplayType.Practice)
        Return dtPractice
    End Function
#End Region
#Region "Get Data from UI"
    ' Get Follow up Action from input
    Public Function GetFollowActionFromInput(ByVal repeaterFollowupAction As Repeater, Optional ByVal formatDate As Boolean = True, Optional ByVal needOrder As Boolean = True) As DataTable
        Dim udtformatter As New Common.Format.Formatter
        'Get data from repeater
        Dim txtActionDate As TextBox
        Dim txtActionDesc As TextBox

        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("Followup_Action_Seq", GetType(System.Int32))
        dt.Columns.Add("Action_Date", GetType(System.String))
        dt.Columns.Add("Action_Date_Format", GetType(System.String))
        dt.Columns.Add("Action_Desc", GetType(System.String))
        dt.TableName = "Root"
        For i As Integer = 0 To repeaterFollowupAction.Items.Count - 1
            If True Then
                txtActionDate = CType(repeaterFollowupAction.Items(i).FindControl("txtFollowUpDate"), TextBox)
                txtActionDesc = CType(repeaterFollowupAction.Items(i).FindControl("txtFollowUpAction"), TextBox)
                Dim dr As DataRow = dt.NewRow()
                dr("Followup_Action_Seq") = i + 1
                dr("Action_Date") = txtActionDate.Text
                dr("Action_Date_Format") = IIf(formatDate, udtformatter.convertDate(txtActionDate.Text, ""), txtActionDate.Text)
                dr("Action_Desc") = txtActionDesc.Text
                dt.Rows.Add(dr)
            End If
        Next
        If needOrder Then
            Dim dv As DataView = dt.DefaultView
            dv.Sort = "Action_Date"
            dt = dv.ToTable
        End If

        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i)("Followup_Action_Seq") = i + 1
        Next
        Return dt

    End Function
    ' Get Follow up Action Structure
    Public Function GetFollowUpActionStructure() As DataTable
        Dim dt As System.Data.DataTable = New System.Data.DataTable()
        dt.Columns.Add("Inspection_ID", GetType(System.String))
        dt.Columns.Add("Followup_Action_Seq", GetType(System.Int32))
        dt.Columns.Add("Action_Date", GetType(System.String))
        dt.Columns.Add("Action_Date_Format", GetType(System.String))
        dt.Columns.Add("Action_Desc", GetType(System.String))
        Return dt
    End Function
    'Get Type of Inspection Seleted Values DataTable
    Public Function GetTypeofInspectionFromInput(ByVal chkList As CheckBoxList) As DataTable
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("Type_of_Inspection", GetType(System.String))

        For Each selectitem As ListItem In chkList.Items
            If selectitem.Selected = True And Not String.IsNullOrEmpty(selectitem.Value) Then
                Dim dr As DataRow = dt.NewRow()
                dr("Type_of_Inspection") = selectitem.Value
                dt.Rows.Add(dr)
            End If
        Next
        If dt.Rows.Count > 0 Then
            dt.TableName = "Root"
        End If
        Return dt
    End Function
    'Get Type of Inspection Seleted Values String
    Public Function GetTypeofInspectionStringFromInput(ByVal chkBoxList As CheckBoxList) As String
        Dim strTypyOfInspection As String = ""
        Dim listTypeOfInspection As New List(Of TypeOfInspectionItem)
        For Each selectitem As ListItem In chkBoxList.Items
            If selectitem.Selected = True And Not String.IsNullOrEmpty(selectitem.Value) Then
                listTypeOfInspection.Add(New TypeOfInspectionItem With {
                                         .Title = selectitem.Value
                                         })
                strTypyOfInspection += IIf(strTypyOfInspection = "", selectitem.Value, "," + selectitem.Value)
            End If
        Next
        Return strTypyOfInspection
    End Function
    'Generate XML String By Type of Inspection String
    Public Function GenXMLTypeOfInspectionByString(strTypyOfInspection As String) As String
        Dim xmlResult As String = ""

        If Not String.IsNullOrEmpty(strTypyOfInspection) Then
            Dim dt As DataTable = New DataTable()
            dt.Columns.Add("Type_of_Inspection", GetType(System.String))
            dt.TableName = "Root"
            For Each Item As String In strTypyOfInspection.Split(",")
                Dim dr As DataRow = dt.NewRow()
                dr("Type_of_Inspection") = Item
                dt.Rows.Add(dr)
            Next

            Dim ms As MemoryStream = Nothing
            Dim XmlWt As XmlTextWriter = Nothing
            ms = New MemoryStream()
            XmlWt = New XmlTextWriter(ms, System.Text.Encoding.Unicode)
            dt.TableName = "TypeOfInspection"
            dt.WriteXml(XmlWt, XmlWriteMode.IgnoreSchema)
            Dim count As Integer = CInt(ms.Length)
            Dim temp As Byte() = New Byte(count - 1) {}
            ms.Seek(0, SeekOrigin.Begin)
            ms.Read(temp, 0, count)
            Dim ucode As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            xmlResult = ucode.GetString(temp).Trim
            If XmlWt IsNot Nothing Then
                XmlWt.Close()
                ms.Close()
                ms.Dispose()
            End If
        End If
        Return xmlResult
    End Function
    'Get Valid Follow Action from input
    Public Function GetValidFollowActionFromInput(ByVal repeaterFollowAction As Repeater, Optional formatDate As Boolean = True) As DataTable
        Dim udtformatter As New Common.Format.Formatter
        'Get data from repeater
        Dim txtActionDate As TextBox
        Dim txtActionDesc As TextBox

        Dim dt As System.Data.DataTable = New System.Data.DataTable()
        dt.Columns.Add("Followup_Action_Seq", GetType(System.Int32))
        dt.Columns.Add("Action_Date", GetType(System.String))
        dt.Columns.Add("Action_Date_Value", GetType(System.String))
        dt.Columns.Add("Action_Desc", GetType(System.String))
        dt.Columns.Add("Action_Date_Format", GetType(System.String))
        dt.TableName = "Root"
        Dim noCount As Integer = 0
        For i As Integer = 0 To repeaterFollowAction.Items.Count - 1
            If True Then
                txtActionDate = CType(repeaterFollowAction.Items(i).FindControl("txtFollowUpDate"), TextBox)
                txtActionDesc = CType(repeaterFollowAction.Items(i).FindControl("txtFollowUpAction"), TextBox)
                If (Not String.IsNullOrEmpty(txtActionDate.Text) And Not String.IsNullOrEmpty(txtActionDesc.Text)) Then
                    Dim dr As DataRow = dt.NewRow()
                    dr("Followup_Action_Seq") = noCount + 1
                    dr("Action_Date_Format") = IIf(formatDate, udtformatter.convertDate(txtActionDate.Text, ""), txtActionDate.Text)
                    dr("Action_Date") = txtActionDate.Text
                    dr("Action_Date_Value") = ConvertDate(txtActionDate.Text).ToString("yyyyMMdd")
                    dr("Action_Desc") = txtActionDesc.Text
                    dt.Rows.Add(dr)
                    noCount += 1
                End If
            End If
        Next
        Dim dv As DataView = dt.DefaultView
        dv.Sort = "Action_Date_Value"
        dt = dv.ToTable
        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i)("Followup_Action_Seq") = i + 1
        Next
        Return dt
    End Function
    'Get Follow up Action From XML String
    Public Function GetFollowupActionFromXML(strXML As String) As DataTable
        Dim dt As DataTable = GetFollowUpActionStructure()

        If Not String.IsNullOrEmpty(strXML) Then
            Dim ds As DataSet = XmlFunction.Xml2Dataset(strXML)
            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)
                Dim dv As DataView = dt.DefaultView
                dv.Sort = "Action_Date"
                dt = dv.ToTable
                dt.Columns.Add("Action_Date_Format", GetType(System.String))
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim dateAction As Date = CDate(dt.Rows(i)("Action_Date"))
                    dt.Rows(i)("Followup_Action_Seq") = i + 1
                    dt.Rows(i)("Action_Date") = FormatInputDate(dateAction)
                    dt.Rows(i)("Action_Date_Format") = FormatOutputDate(dateAction)
                Next
            End If
        End If
        Return dt
    End Function
    'Generate Further Action Datatable
    Public Function GenFurtherActionTableData(ByVal AList As List(Of FurtherActionItem), ByVal RList As List(Of FurtherActionItem), ByVal IList As List(Of FurtherActionItem)) As DataTable
        Dim furtherActionDT As New DataTable
        furtherActionDT.Columns.Add("Rowspan", GetType(Integer))
        furtherActionDT.Columns.Add("Action", GetType(String))
        furtherActionDT.Columns.Add("Type", GetType(String))
        furtherActionDT.Columns.Add("Date", GetType(String))
        Dim furtherDr As DataRow
        Dim intCount As Integer = IList.Count
        For Each Item As FurtherActionItem In IList
            furtherDr = furtherActionDT.NewRow
            furtherDr("Rowspan") = intCount
            intCount = 0
            furtherDr("Action") = Item.Action
            furtherDr("Type") = Item.ActionType
            furtherDr("Date") = Item.ActionDate
            furtherActionDT.Rows.Add(furtherDr)
        Next
        intCount = RList.Count
        For Each Item As FurtherActionItem In RList
            furtherDr = furtherActionDT.NewRow
            furtherDr("Rowspan") = intCount
            intCount = 0
            furtherDr("Action") = Item.Action
            furtherDr("Type") = Item.ActionType
            furtherDr("Date") = Item.ActionDate
            furtherActionDT.Rows.Add(furtherDr)
        Next
        intCount = AList.Count
        For Each Item As FurtherActionItem In AList
            furtherDr = furtherActionDT.NewRow
            furtherDr("Rowspan") = intCount
            intCount = 0
            furtherDr("Action") = Item.Action
            furtherDr("Type") = Item.ActionType
            furtherDr("Date") = Item.ActionDate
            furtherActionDT.Rows.Add(furtherDr)
        Next
        Return furtherActionDT
    End Function

#End Region
#Region "Create Inspection Record"
    'Create new inspection record
    Public Sub CreateInspectionRecord(ByRef model As InspectionRecordModel, Optional ByRef udtDB As Database = Nothing)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        If udtDB IsNot Nothing Then
            db = udtDB
        End If

        'Generate Inspection ID
        Dim inspID = udtGeneralFunction.GenerateInspectionRecordID(udtDB)
        model.InspectionID = inspID

        If (model.FileReferenceType = FileReferenceType.NewFile) Then
            model.FileReferenceNo = udtGeneralFunction.GenerateInspectionFileRefNo(model.FileReferenceNo, db)
        End If

        Dim dt As New DataTable
        Dim params() As SqlParameter = New SqlParameter() {
            db.MakeInParam("@File_Reference_No", InspectionRecordModel.FileReferenceNoDataType, InspectionRecordModel.FileReferenceNoDataSize,
                           model.FileReferenceNo),
            db.MakeInParam("@File_Reference_Type", InspectionRecordModel.FileReferenceNoDataType, InspectionRecordModel.FileReferenceNoDataSize,
                           model.FileReferenceType),
            db.MakeInParam("@Inspection_ID", InspectionRecordModel.InspectionIDDataType, InspectionRecordModel.InspectionIDDataSize,
                           model.InspectionID),
            db.MakeInParam("@Referred_Reference_No_1", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           model.ReferredReferenceNo1),
            db.MakeInParam("@Referred_Reference_No_2", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           model.ReferredReferenceNo2),
            db.MakeInParam("@Referred_Reference_No_3", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           model.ReferredReferenceNo3),
            db.MakeInParam("@SP_ID", InspectionRecordModel.SPIDDataType, InspectionRecordModel.SPIDDataSize,
                           IIf(model.SPID = String.Empty, DBNull.Value, model.SPID)),
            db.MakeInParam("@Practice_Display_Seq", InspectionRecordModel.PracticeDisplaySeqDataType, InspectionRecordModel.PracticeDisplaySeqDataSize,
                           model.PracticeDisplaySeq),
            db.MakeInParam("@Main_Type_Of_Inspection", InspectionRecordModel.MainTypeOfInspectionDataType, InspectionRecordModel.MainTypeOfInspectionDataSize,
                           model.MainTypeOfInspectionID),
            db.MakeInParam("@Type_Of_Inspection", InspectionRecordModel.TypeOfInspectionDataType, InspectionRecordModel.TypeOfInspectionDataSize,
                           model.OtherTypeOfInspectionID),
            db.MakeInParam("@Visit_Date", InspectionRecordModel.VisitDateDataType, InspectionRecordModel.VisitDateDataSize,
                           IIf(model.VisitDate = DateTime.MinValue, DBNull.Value, model.VisitDate)),
            db.MakeInParam("@Visit_Begin_Dtm", InspectionRecordModel.VisitBeginDtmDataType, InspectionRecordModel.VisitBeginDtmDataSize,
                           IIf(model.VisitBeginDtm = DateTime.MinValue, DBNull.Value, model.VisitBeginDtm)),
            db.MakeInParam("@Visit_End_Dtm", InspectionRecordModel.VisitEndDtmDataType, InspectionRecordModel.VisitEndDtmDataSize,
                           IIf(model.VisitEndDtm = DateTime.MinValue, DBNull.Value, model.VisitEndDtm)),
            db.MakeInParam("@Confirmation_with", InspectionRecordModel.ConfirmationwithDataType, InspectionRecordModel.ConfirmationwithDataSize,
                           IIf(model.ConfirmationWith = String.Empty, DBNull.Value, model.ConfirmationWith)),
            db.MakeInParam("@Confirmation_Dtm", InspectionRecordModel.ConfirmationDtmDataType, InspectionRecordModel.ConfirmationDtmDataSize,
                           IIf(model.ConfirmationDtm = DateTime.MinValue, DBNull.Value, model.ConfirmationDtm)),
            db.MakeInParam("@Form_Condition", InspectionRecordModel.FormConditionDataType, InspectionRecordModel.FormConditionDataSize,
                           IIf(model.FormConditionID = String.Empty, DBNull.Value, model.FormConditionID)),
            db.MakeInParam("@Form_Condition_Remark", InspectionRecordModel.FormConditionRemarkDataType, InspectionRecordModel.FormConditionRemarkDataSize,
                          IIf(model.FormConditionRemark = String.Empty, DBNull.Value, model.FormConditionRemark)),
            db.MakeInParam("@Means_Of_Communication", InspectionRecordModel.MeansOfCommunicationDataType, InspectionRecordModel.MeansOfCommunicationDataSize,
                          IIf(model.MeansOfCommunicationID Is String.Empty, DBNull.Value, model.MeansOfCommunicationID)),
            db.MakeInParam("@Means_Of_Communication_Fax", InspectionRecordModel.MeansOfCommunicationFaxDataType, InspectionRecordModel.MeansOfCommunicationFaxDataSize,
                          IIf(model.MeansOfCommunicationFax Is String.Empty, DBNull.Value, model.MeansOfCommunicationFax)),
            db.MakeInParam("@Means_Of_Communication_Email", InspectionRecordModel.MeansOfCommunicationEmailDataType, InspectionRecordModel.MeansOfCommunicationEmailDataSize,
                          IIf(model.MeansOfCommunicationEmail Is String.Empty, DBNull.Value, model.MeansOfCommunicationEmail)),
            db.MakeInParam("@Low_Risk_Claim", InspectionRecordModel.LowRiskClaimDataType, InspectionRecordModel.LowRiskClaimDataSize,
                           IIf(String.IsNullOrEmpty(model.LowRiskClaim), DBNull.Value, model.LowRiskClaim)),
            db.MakeInParam("@Remarks", InspectionRecordModel.RemarksDataType, InspectionRecordModel.RemarksDataSize,
                           IIf(model.Remarks = String.Empty, DBNull.Value, model.Remarks)),
            db.MakeInParam("@Case_Officer", InspectionRecordModel.CaseOfficerDataType, InspectionRecordModel.CaseOfficerDataSize,
                           IIf(model.CaseOfficerID = String.Empty, DBNull.Value, model.CaseOfficerID)),
            db.MakeInParam("@Case_Contact_No", InspectionRecordModel.ContactNoDataType, InspectionRecordModel.ContactNoDataSize,
                           IIf(model.CaseOfficerContactNo = String.Empty, DBNull.Value, model.CaseOfficerContactNo)),
            db.MakeInParam("@Subject_Officer", InspectionRecordModel.SubjectOfficerDataType, InspectionRecordModel.SubjectOfficerDataSize,
                           IIf(model.SubjectOfficerID = String.Empty, DBNull.Value, model.SubjectOfficerID)),
            db.MakeInParam("@Subject_Contact_No", InspectionRecordModel.ContactNoDataType, InspectionRecordModel.ContactNoDataSize,
                           IIf(model.SubjectOfficerContactNo = String.Empty, DBNull.Value, model.SubjectOfficerContactNo)),
            db.MakeInParam("@User_ID", InspectionRecordModel.UserIDDataType, InspectionRecordModel.UserIDDataSize,
                           IIf(model.UserID = String.Empty, DBNull.Value, model.UserID)),
            db.MakeInParam("@Record_Status", InspectionRecordModel.RecordStatusDataType, InspectionRecordModel.RecordStatusDataSize,
                           model.RecordStatus),
             db.MakeInParam("@Service_Category_Code", InspectionRecordModel.ServiceCategoryCodeDataType, InspectionRecordModel.ServiceCategoryCodeDataSize,
                           model.ServiceCategoryCode)
        }
        db.RunProc("proc_InspectionVisitInfo_add", params)

    End Sub
#End Region
#Region "Update Inspection Record"
    'Update inspection record
    Public Function UpdateRecord(model As InspectionRecordModel, ByVal UpdateType As String) As Boolean
        Dim params() As SqlParameter = {
            db.MakeInParam("@File_Reference_No", InspectionRecordModel.FileReferenceNoDataType, InspectionRecordModel.FileReferenceNoDataSize,
                           IIf(model.FileReferenceNo = String.Empty, "", model.FileReferenceNo)),
            db.MakeInParam("@Inspection_ID", InspectionRecordModel.InspectionIDDataType, InspectionRecordModel.InspectionIDDataSize,
                           model.InspectionID),
            db.MakeInParam("@Referred_Reference_No_1", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           IIf(model.ReferredReferenceNo1 = String.Empty, DBNull.Value, model.ReferredReferenceNo1)),
            db.MakeInParam("@Referred_Reference_No_2", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           IIf(model.ReferredReferenceNo2 = String.Empty, DBNull.Value, model.ReferredReferenceNo2)),
            db.MakeInParam("@Referred_Reference_No_3", InspectionRecordModel.ReferredReferenceNoDataType, InspectionRecordModel.ReferredReferenceNoDataSize,
                           IIf(model.ReferredReferenceNo3 = String.Empty, DBNull.Value, model.ReferredReferenceNo3)),
            db.MakeInParam("@Type_Of_Inspection", InspectionRecordModel.TypeOfInspectionDataType, InspectionRecordModel.TypeOfInspectionDataSize,
                           IIf(model.OtherTypeOfInspectionID = String.Empty, "", model.OtherTypeOfInspectionID)),
            db.MakeInParam("@Visit_Date", InspectionRecordModel.VisitDateDataType, InspectionRecordModel.VisitDateDataSize,
                           IIf(model.VisitDate = DateTime.MinValue, DBNull.Value, model.VisitDate)),
            db.MakeInParam("@Visit_Begin_Dtm", InspectionRecordModel.VisitBeginDtmDataType, InspectionRecordModel.VisitBeginDtmDataSize,
                           IIf(model.VisitBeginDtm = DateTime.MinValue, DBNull.Value, model.VisitBeginDtm)),
            db.MakeInParam("@Visit_End_Dtm", InspectionRecordModel.VisitEndDtmDataType, InspectionRecordModel.VisitEndDtmDataSize,
                           IIf(model.VisitEndDtm = DateTime.MinValue, DBNull.Value, model.VisitEndDtm)),
            db.MakeInParam("@Confirmation_with", InspectionRecordModel.ConfirmationwithDataType, InspectionRecordModel.ConfirmationwithDataSize,
                           IIf(model.ConfirmationWith = String.Empty, DBNull.Value, model.ConfirmationWith)),
            db.MakeInParam("@Confirmation_Dtm", InspectionRecordModel.ConfirmationDtmDataType, InspectionRecordModel.ConfirmationDtmDataSize,
                           IIf(model.ConfirmationDtm = DateTime.MinValue, DBNull.Value, model.ConfirmationDtm)),
            db.MakeInParam("@Form_Condition", InspectionRecordModel.FormConditionDataType, InspectionRecordModel.FormConditionDataSize,
                           IIf(model.FormConditionID = String.Empty, DBNull.Value, model.FormConditionID)),
            db.MakeInParam("@Form_Condition_Remark", InspectionRecordModel.FormConditionRemarkDataType, InspectionRecordModel.FormConditionRemarkDataSize,
                           IIf(model.FormConditionRemark = String.Empty, DBNull.Value, model.FormConditionRemark)),
            db.MakeInParam("@Means_Of_Communication", InspectionRecordModel.MeansOfCommunicationDataType, InspectionRecordModel.MeansOfCommunicationDataSize,
                           IIf(model.MeansOfCommunicationID = String.Empty, DBNull.Value, model.MeansOfCommunicationID)),
            db.MakeInParam("@Means_Of_Communication_Fax", InspectionRecordModel.MeansOfCommunicationFaxDataType, InspectionRecordModel.MeansOfCommunicationFaxDataSize,
                           IIf(model.MeansOfCommunicationFax = String.Empty, DBNull.Value, model.MeansOfCommunicationFax)),
            db.MakeInParam("@Means_Of_Communication_Email", InspectionRecordModel.MeansOfCommunicationEmailDataType, InspectionRecordModel.MeansOfCommunicationEmailDataSize,
                           IIf(model.MeansOfCommunicationEmail = String.Empty, DBNull.Value, model.MeansOfCommunicationEmail)),
            db.MakeInParam("@Low_Risk_Claim", InspectionRecordModel.LowRiskClaimDataType, InspectionRecordModel.LowRiskClaimDataSize,
                           IIf(String.IsNullOrEmpty(model.LowRiskClaim), DBNull.Value, model.LowRiskClaim)),
            db.MakeInParam("@Remarks", InspectionRecordModel.RemarksDataType, InspectionRecordModel.RemarksDataSize,
                           IIf(model.Remarks = String.Empty, DBNull.Value, model.Remarks)),
            db.MakeInParam("@Case_Officer", InspectionRecordModel.CaseOfficerDataType, InspectionRecordModel.CaseOfficerDataSize,
                           IIf(model.CaseOfficerID = String.Empty, DBNull.Value, model.CaseOfficerID)),
            db.MakeInParam("@Case_Contact_No", InspectionRecordModel.ContactNoDataType, InspectionRecordModel.ContactNoDataSize,
                           IIf(model.CaseOfficerContactNo = String.Empty, DBNull.Value, model.CaseOfficerContactNo)),
            db.MakeInParam("@Subject_Officer", InspectionRecordModel.SubjectOfficerDataType, InspectionRecordModel.SubjectOfficerDataSize,
                           IIf(model.SubjectOfficerID = String.Empty, DBNull.Value, model.SubjectOfficerID)),
            db.MakeInParam("@Subject_Contact_No", InspectionRecordModel.ContactNoDataType, InspectionRecordModel.ContactNoDataSize,
                           IIf(model.SubjectOfficerContactNo = String.Empty, DBNull.Value, model.SubjectOfficerContactNo)),
            db.MakeInParam("@No_Of_InOrder", InspectionRecordModel.NoOfInOrderDataType, InspectionRecordModel.NoOfInOrderDataSize,
                           model.NoOfInOrder),
            db.MakeInParam("@No_Of_MissingForm", InspectionRecordModel.NoOfMissingFormDataType, InspectionRecordModel.NoOfMissingFormDataSize,
                           model.NoOfMissingForm),
            db.MakeInParam("@No_Of_Inconsistent", InspectionRecordModel.NoOfInconsistentDataType, InspectionRecordModel.NoOfInconsistentDataSize,
                           model.NoOfInconsistent),
            db.MakeInParam("@No_Of_TotalCheck", InspectionRecordModel.NoOfTotalCheckDataType, InspectionRecordModel.NoOfTotalCheckDataSize,
                           model.NoOfTotalCheck),
            db.MakeInParam("@Checking_Date", InspectionRecordModel.CheckingDateDataType, InspectionRecordModel.CheckingDateDataSize,
                           IIf(model.CheckingDate = DateTime.MinValue, DBNull.Value, model.CheckingDate)),
            db.MakeInParam("@Anomalous_Claims", InspectionRecordModel.AnomalousClaimsDataType, InspectionRecordModel.AnomalousClaimsDataSize,
                          IIf(String.IsNullOrEmpty(model.AnomalousClaims), DBNull.Value, model.AnomalousClaims)),
            db.MakeInParam("@Is_OverMajor", InspectionRecordModel.IsOverMajorDataType, InspectionRecordModel.IsOverMajorDataSize,
                           IIf(String.IsNullOrEmpty(model.IsOverMajor), DBNull.Value, model.IsOverMajor)),
            db.MakeInParam("@No_Of_Anomalous_Claims", InspectionRecordModel.NoOfInconsistentDataType, InspectionRecordModel.NoOfInconsistentDataSize,
                           model.NoOfAnomalousClaims),
            db.MakeInParam("@No_Of_Is_OverMajor", InspectionRecordModel.NoOfInconsistentDataType, InspectionRecordModel.NoOfInconsistentDataSize,
                           model.NoOfIsOverMajor),
            db.MakeInParam("@Advisory_Letter_Date", InspectionRecordModel.AdvisoryLetterDateDataType, InspectionRecordModel.AdvisoryLetterDateDataSize,
                           DateMinvalueToDBNull(model.AdvisoryLetterDate)),
            db.MakeInParam("@Warning_Letter_Date", InspectionRecordModel.WarningLetterDateDataType, InspectionRecordModel.WarningLetterDateDataSize,
                           DateMinvalueToDBNull(model.WarningLetterDate)),
            db.MakeInParam("@Delist_Letter_Date", InspectionRecordModel.DelistLetterDateDataType, InspectionRecordModel.DelistLetterDateDataSize,
                           DateMinvalueToDBNull(model.DelistLetterDate)),
            db.MakeInParam("@Other_Letter_Date", InspectionRecordModel.OtherLetterDateDataType, InspectionRecordModel.OtherLetterDateDataSize,
                           DateMinvalueToDBNull(model.OtherLetterDate)),
            db.MakeInParam("@Suspend_Payment_Letter_Date", InspectionRecordModel.OtherLetterDateDataType, InspectionRecordModel.OtherLetterDateDataSize,
                           DateMinvalueToDBNull(model.SuspendPaymentLetterDate)),
            db.MakeInParam("@Suspend_EHCP_Account_Letter_Date", InspectionRecordModel.OtherLetterDateDataType, InspectionRecordModel.OtherLetterDateDataSize,
                           DateMinvalueToDBNull(model.SuspendEHCPAccountLetterDate)),
            db.MakeInParam("@Other_Letter_Remark", InspectionRecordModel.OtherLetterRemarkDataType, InspectionRecordModel.OtherLetterRemarkDataSize,
                           IIf(model.OtherLetterRemark = String.Empty, DBNull.Value, model.OtherLetterRemark)),
            db.MakeInParam("@BoardAndCouncil_Date", InspectionRecordModel.BoardAndCouncilDateDataType, InspectionRecordModel.BoardAndCouncilDateDataSize,
                           DateMinvalueToDBNull(model.BoardAndCouncilDate)),
            db.MakeInParam("@Police_Date", InspectionRecordModel.PoliceDateDataType, InspectionRecordModel.PoliceDateDataSize,
                           DateMinvalueToDBNull(model.PoliceDate)),
            db.MakeInParam("@Social_Welfare_Department_Date", InspectionRecordModel.PoliceDateDataType, InspectionRecordModel.PoliceDateDataSize,
                           DateMinvalueToDBNull(model.SocialWelfareDepartmentDate)),
            db.MakeInParam("@HK_Customs_And_Excise_Department_Date", InspectionRecordModel.PoliceDateDataType, InspectionRecordModel.PoliceDateDataSize,
                           DateMinvalueToDBNull(model.HKCustomsandExciseDepartmentDate)),
            db.MakeInParam("@Immigration_Department_Date", InspectionRecordModel.PoliceDateDataType, InspectionRecordModel.PoliceDateDataSize,
                           DateMinvalueToDBNull(model.ImmigrationDepartmentDate)),
            db.MakeInParam("@Labour_Department_Date", InspectionRecordModel.PoliceDateDataType, InspectionRecordModel.PoliceDateDataSize,
                           DateMinvalueToDBNull(model.LabourDeparmentDate)),
            db.MakeInParam("@Other_Party_Date", InspectionRecordModel.OtherPartyDateDataType, InspectionRecordModel.OtherPartyDateDataSize,
                           DateMinvalueToDBNull(model.OtherPartyDate)),
            db.MakeInParam("@Other_Party_Remark", InspectionRecordModel.OtherPartyRemarkDataType, InspectionRecordModel.OtherPartyRemarkDataSize,
                           IIf(model.OtherPartyRemark = String.Empty, DBNull.Value, model.OtherPartyRemark)),
            db.MakeInParam("@Suspend_EHCP_Date", InspectionRecordModel.DelistEHCPDateDataType, InspectionRecordModel.DelistEHCPDateDataSize,
                           DateMinvalueToDBNull(model.SuspendEHCPDate)),
            db.MakeInParam("@Delist_EHCP_Date", InspectionRecordModel.DelistEHCPDateDataType, InspectionRecordModel.DelistEHCPDateDataSize,
                           DateMinvalueToDBNull(model.DelistEHCPDate)),
            db.MakeInParam("@Payment_RecoverySuspension_Date", InspectionRecordModel.PaymentRecoverySuspensionDateDataType, InspectionRecordModel.PaymentRecoverySuspensionDateDataSize,
                           DateMinvalueToDBNull(model.PaymentRecoverySuspensionDate)),
            db.MakeInParam("@Followup_Action", InspectionRecordModel.FollowupActionDataType, InspectionRecordModel.FollowupActionDataSize,
                           IIf(model.FollowupAction = String.Empty, DBNull.Value, model.FollowupAction)),
            db.MakeInParam("@Record_Status", InspectionRecordModel.RecordStatusDataType, InspectionRecordModel.RecordStatusDataSize,
                           IIf(model.RecordStatus = String.Empty, DBNull.Value, model.RecordStatus)),
            db.MakeInParam("@Original_Status", InspectionRecordModel.OriginalStatusDataType, InspectionRecordModel.OriginalStatusDataSize,
                           IIf(model.OriginalStatus = String.Empty, DBNull.Value, model.OriginalStatus)),
            db.MakeInParam("@Userid", InspectionRecordModel.UserIDDataType, InspectionRecordModel.UserIDDataSize,
                           model.UserID),
            db.MakeInParam("@TSMP", InspectionRecordModel.TSMPDataType, InspectionRecordModel.TSMPDataSize,
                           model.TSMP),
            db.MakeInParam("@UpdateType", SqlDbType.VarChar, 30,
                           UpdateType),
            db.MakeInParam("@SP_ID", InspectionRecordModel.SPIDDataType, InspectionRecordModel.SPIDDataSize,
                           IIf(model.SPID = String.Empty, DBNull.Value, model.SPID)),
            db.MakeInParam("@Practice_Display_Seq", InspectionRecordModel.PracticeDisplaySeqDataType, InspectionRecordModel.PracticeDisplaySeqDataSize,
                           model.PracticeDisplaySeq),
            db.MakeInParam("@Service_Category_Code", InspectionRecordModel.ServiceCategoryCodeDataType, InspectionRecordModel.ServiceCategoryCodeDataSize,
                            IIf(model.ServiceCategoryCode = String.Empty, DBNull.Value, model.ServiceCategoryCode)),
            db.MakeInParam("@Last_Visit_Date", InspectionRecordModel.LastVisitDateDataType, InspectionRecordModel.LastVisitDateDataSize,
                           IIf(model.SPLastVisitDate = DateTime.MinValue, DBNull.Value, model.SPLastVisitDate)),
            db.MakeInParam("@Request_Reopen_Reason", InspectionRecordModel.FormConditionRemarkDataType, InspectionRecordModel.FormConditionRemarkDataSize,
                           IIf(model.ReopenRequestReason = String.Empty, DBNull.Value, model.ReopenRequestReason))
        }
        Try
            db.BeginTransaction()
            db.RunProc("proc_InspectionVisitInfo_upd", params)
            db.CommitTransaction()
        Catch eSQL As SqlException
            db.RollBackTranscation()
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message
                If strmsg = MsgCode.MSG00011 Then
                    ErrorHandler.Log("", SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, eSQL.Message)
                    Return False
                End If
            End If
            Throw
        Catch ex As Exception
            db.RollBackTranscation()
            Throw
        End Try
        Return True
    End Function
#End Region
#Region "UI Handle Function"
    'Handle Main Type of Inspection Selected Value Change
    Public Sub HandleMainTypeOfInspectionSelectedValue(ByVal strTypeOfInspection As String, dataField As DetailDataField)

        dataField.chkListTypeOfInspection.Enabled = True
        HandleTextBoxEnable(dataField.txtReferFileRefNoA1, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoA2, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoA3, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoA4, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoA5, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoB1, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoB2, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoB3, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoB4, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoB5, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoC1, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoC2, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoC3, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoC4, True)
        HandleTextBoxEnable(dataField.txtReferFileRefNoC5, True)

        Select Case strTypeOfInspection
            Case TypeOfInspection.Routine
                dataField.chkListTypeOfInspection.ClearSelection()
                dataField.chkListTypeOfInspection.Enabled = False
            Case TypeOfInspection.RoutineNew
                HandleTextBoxEnable(dataField.txtReferFileRefNoA1, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoA2, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoA3, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoA4, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoA5, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoB1, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoB2, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoB3, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoB4, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoB5, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoC1, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoC2, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoC3, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoC4, False)
                HandleTextBoxEnable(dataField.txtReferFileRefNoC5, False)
        End Select
    End Sub
    'Handle TextBox Enable
    Public Sub HandleTextBoxEnable(ByVal textBox As TextBox, ByVal enable As Boolean)
        If Not enable Then
            textBox.Text = ""
        End If
        textBox.Enabled = enable
    End Sub
    'File Reference No
    Public Sub SplitReferNo(ByVal referNo As String, ByRef partA As String, ByRef partB As String, ByRef partC As String, ByRef partD As String, ByRef partE As String)
        Dim arrPart1 As String() = referNo.Split("/"c)
        Dim strTemp As String
        strTemp = arrPart1(arrPart1.Length - 1)
        partA = arrPart1(arrPart1.Length - 2)
        Dim arrSubPart1 As String() = strTemp.Split("-"c)
        partB = arrSubPart1(0)
        partC = arrSubPart1(1)
        partD = arrSubPart1(2)
        If (arrSubPart1.Length > 3) Then
            partE = arrSubPart1(3).Replace("(", "").Replace(")", "")
        Else
            partE = ""
        End If
    End Sub
    Public Function HandleFileRefNo(ByVal txt1 As String, ByVal txt2 As String, ByVal txt3 As String, ByVal txt4 As String, ByVal txt5 As String) As String
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim referFileRefNo As String = ""
        If Not String.IsNullOrEmpty(txt1) And Not String.IsNullOrEmpty(txt2) And Not String.IsNullOrEmpty(txt3) And Not String.IsNullOrEmpty(txt4) Then
            referFileRefNo = udtGeneralFunction.getSystemParameter("Inspection_FileRefNo_Prefix") + txt1 + "/" + txt2 + "-" + txt3 + "-" + txt4 + IIf(String.IsNullOrEmpty(txt5), "", "-(" + (txt5) + ")")
        End If
        Return referFileRefNo
    End Function

    Public Function CheckButtonEnable(ByVal btn As ImageButton) As SystemMessage
        Dim errMsg As SystemMessage = Nothing
        If Not btn.Enabled Then
            'Access Denied. You do not have permission to access this page'
            errMsg = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00448)
        End If
        Return errMsg
    End Function
#End Region

    'Serialize Function
    Private Function SerializeInspectionRecord(ByVal dr As DataRow) As InspectionRecordModel
        Dim model As New InspectionRecordModel
        'Inspection Record
        With model
            .InspectionID = FieldValueToString(dr("Inspection_ID"))
            .CreateBy = FieldValueToString(dr("Create_By"))
            .CreateDtm = FieldValueToDate(dr("Create_Dtm"))
            .UpdateBy = FieldValueToString(dr("Update_By"))
            .UpdateDtm = FieldValueToDate(dr("Update_Dtm"))
            .RemoveRequestBy = FieldValueToString(dr("Request_Remove_By"))
            .RemoveRequestDtm = FieldValueToDate(dr("Request_Remove_Dtm"))
            .CloseRequestBy = FieldValueToString(dr("Request_Close_By"))
            .CloseRequestDtm = FieldValueToDate(dr("Request_Close_Dtm"))
            .ReopenRequestBy = FieldValueToString(dr("Request_Reopen_By"))
            .ReopenRequestDtm = FieldValueToDate(dr("Request_Reopen_Dtm"))
            .ReopenRequestReason = FieldValueToString(dr("Request_Reopen_Reason"))
            .RemoveApproveBy = FieldValueToString(dr("Approve_Remove_By"))
            .RemoveApproveDtm = FieldValueToDate(dr("Approve_Remove_Dtm"))
            .CloseApproveBy = FieldValueToString(dr("Approve_Close_By"))
            .CloseApproveDtm = FieldValueToDate(dr("Approve_Close_Dtm"))
            .ReopenApproveBy = FieldValueToString(dr("Approve_Reopen_By"))
            .ReopenApproveDtm = FieldValueToDate(dr("Approve_Reopen_Dtm"))

            .OriginalStatus = FieldValueToString(dr("Original_Status"))
            .RecordStatus = FieldValueToString(dr("Record_Status"))
            .RecordStatusValue = FieldValueToString(dr("Record_Status_Value"))
            .OtherTypeOfInspectionID = FieldValueToString(dr("Type_Of_Inspection_ID"))
            .OtherTypeOfInspectionValue = FieldValueToString(dr("Type_Of_Inspection_Value"))
            .MainTypeOfInspectionID = FieldValueToString(dr("Main_Type_Of_Inspection_ID"))
            .MainTypeOfInspectionValue = FieldValueToString(dr("Main_Type_Of_Inspection_Value"))
            .FileReferenceNo = FieldValueToString(dr("File_Reference_No"))
            .FileReferenceType = FieldValueToString(dr("File_Reference_Type"))
            .ReferredReferenceNo1 = FieldValueToString(dr("Referred_Reference_No_1"))
            .ReferredReferenceNo2 = FieldValueToString(dr("Referred_Reference_No_2"))
            .ReferredReferenceNo3 = FieldValueToString(dr("Referred_Reference_No_3"))
            .ReferredInspectionID1 = FieldValueToString(dr("Referred_Inspection_ID_1"))
            .ReferredInspectionID2 = FieldValueToString(dr("Referred_Inspection_ID_2"))
            .ReferredInspectionID3 = FieldValueToString(dr("Referred_Inspection_ID_3"))
            .CaseOfficerID = FieldValueToString(dr("Case_Officer_ID"))
            .CaseOfficerValue = FieldValueToString(dr("Case_Officer_Value"))
            .CaseOfficerContactNo = FieldValueToString(dr("Case_Officer_Contact_No"))
            .SubjectOfficerID = FieldValueToString(dr("Subject_Officer_ID"))
            .SubjectOfficerValue = FieldValueToString(dr("Subject_Officer_Value"))
            .SubjectOfficerContactNo = FieldValueToString(dr("Subject_Officer_Contact_No"))
            '-----------------------------------------------------------------------------'
            'Visit Target
            .SPID = FieldValueToString(dr("SP_ID"))
            .SPStatus = FieldValueToString(dr("SP_Status"))
            .SPEngName = FieldValueToString(dr("SP_Eng_Name"))
            .SPChiName = FieldValueToString(dr("SP_Chi_Name"))
            .SPTelNo = FieldValueToString(dr("SP_Contact_No"))
            .SPFaxNo = FieldValueToString(dr("SP_Fax"))
            .SPEmail = FieldValueToString(dr("SP_Email"))
            .SPHCVSEffectiveDtm = FieldValueToDate(dr("SP_HCVS_Effective_Dtm"))
            .SPHCVSDHCEffectiveDtm = FieldValueToDate(dr("SP_HCVSDHC_Effective_Dtm"))
            .SPHCVSCHNEffectiveDtm = FieldValueToDate(dr("SP_HCVSCHN_Effective_Dtm"))
            .SPHCVSDelistDtm = FieldValueToDate(dr("SP_HCVS_Delist_Dtm"))
            .SPHCVSDHCDelistDtm = FieldValueToDate(dr("SP_HCVSDHC_Delist_Dtm"))
            .SPHCVSCHNDelistDtm = FieldValueToDate(dr("SP_HCVSCHN_Delist_Dtm"))
            .SPLastVisitDate = FieldValueToDate(dr("SP_Last_Visit_Date"))
            .SPLastVisitFileRefNo = FieldValueToString(dr("SP_Last_Visit_File_Ref_No"))
            .PracticeDisplaySeq = FieldValueToInt(dr("Practice_Display_Seq"))
            .PracticeName = FieldValueToString(dr("Practice_Name"))
            .PracticeNameChi = FieldValueToString(dr("Practice_Name_Chi"))
            .PracticeStatus = FieldValueToString(dr("Practice_Status"))
            .PracticeAddress = FieldValueToString(dr("Practice_Address"))
            .PracticeAddressChi = FieldValueToString(dr("Practice_Address_Chi"))
            .ServiceCategoryCode = FieldValueToString(dr("Service_Category_Code"))
            .ServiceCategoryDesc = FieldValueToString(dr("Service_Category_Desc"))
            .PracticePhoneNo = FieldValueToString(dr("Practice_Contact_No"))
            .PracticeRegCode = FieldValueToString(dr("Practice_Reg_Code"))
            .FreezeDate = FieldValueToDate(dr("Freeze_Date"))
            '-----------------------------------------------------------------------------'
            'Visit Detail
            .VisitDate = FieldValueToDate(dr("Visit_Date"))
            .VisitBeginDtm = FieldValueToDate(dr("Visit_Begin_Dtm"))
            .VisitEndDtm = FieldValueToDate(dr("Visit_End_Dtm"))
            .ConfirmationWith = FieldValueToString(dr("Confirmation_With"))
            .ConfirmationDtm = FieldValueToDate(dr("Confirmation_Dtm"))
            .FormConditionID = FieldValueToString(dr("Form_Condition_ID"))
            .FormConditionValue = FieldValueToString(dr("Form_Condition_Value"))
            .FormConditionRemark = FieldValueToString(dr("Form_Condition_Remark"))
            .MeansOfCommunicationID = FieldValueToString(dr("Means_Of_Communication_ID"))
            .MeansOfCommunicationValue = FieldValueToString(dr("Means_Of_Communication_Value"))
            .MeansOfCommunicationFax = FieldValueToString(dr("Means_Of_Communication_Fax"))
            .MeansOfCommunicationEmail = FieldValueToString(dr("Means_Of_Communication_Email"))
            .LowRiskClaim = FieldValueToString(dr("Low_Risk_Claim"))
            .Remarks = FieldValueToString(dr("Remarks"))
            '-----------------------------------------------------------------------------'
            'Inspection Result
            .NoOfMissingForm = FieldValueToInt(dr("No_Of_MissingForm"))
            .NoOfInconsistent = FieldValueToInt(dr("No_Of_Inconsistent"))
            .NoOfInOrder = FieldValueToInt(dr("No_Of_InOrder"))
            .NoOfTotalCheck = FieldValueToInt(dr("No_Of_TotalCheck"))
            .AnomalousClaims = FieldValueToString(dr("Anomalous_Claims"))
            .NoOfAnomalousClaims = FieldValueToInt(dr("No_Of_Anomalous_Claims"))
            .IsOverMajor = FieldValueToString(dr("Is_OverMajor"))
            .NoOfIsOverMajor = FieldValueToInt(dr("No_Of_Is_OverMajor"))
            .CheckingDate = FieldValueToDate(dr("Checking_Date"))
            '-----------------------------------------------------------------------------'
            'Action - Issue Letter
            .AdvisoryLetterDate = FieldValueToDate(dr("Advisory_Letter_Date"))
            .WarningLetterDate = FieldValueToDate(dr("Warning_Letter_Date"))
            .DelistLetterDate = FieldValueToDate(dr("Delist_Letter_Date"))
            .SuspendPaymentLetterDate = FieldValueToDate(dr("Suspend_Payment_Letter_Date"))
            .SuspendEHCPAccountLetterDate = FieldValueToDate(dr("Suspend_EHCP_Account_Letter_Date"))
            .OtherLetterDate = FieldValueToDate(dr("Other_Letter_Date"))
            .OtherLetterRemark = FieldValueToString(dr("Other_Letter_Remark"))
            '-----------------------------------------------------------------------------'
            'Action - Refer Parties
            .BoardAndCouncilDate = FieldValueToDate(dr("BoardAndCouncil_Date"))
            .PoliceDate = FieldValueToDate(dr("Police_Date"))
            .SocialWelfareDepartmentDate = FieldValueToDate(dr("Social_Welfare_Department_Date"))
            .HKCustomsandExciseDepartmentDate = FieldValueToDate(dr("HK_Customs_And_Excise_Department_Date"))
            .ImmigrationDepartmentDate = FieldValueToDate(dr("Immigration_Department_Date"))
            .LabourDeparmentDate = FieldValueToDate(dr("Labour_Department_Date"))
            .OtherPartyDate = FieldValueToDate(dr("Other_Party_Date"))
            .OtherPartyRemark = FieldValueToString(dr("Other_Party_Remark"))
            '-----------------------------------------------------------------------------'
            'Action - Actions to EHCP
            .SuspendEHCPDate = FieldValueToDate(dr("Suspend_EHCP_Date"))
            .DelistEHCPDate = FieldValueToDate(dr("Delist_EHCP_Date"))
            .PaymentRecoverySuspensionDate = FieldValueToDate(dr("Payment_RecoverySuspension_Date"))
            .RequireFollowup = FieldValueToString(dr("Require_Followup"))
            .FollowupAction = FieldValueToString(dr("Followup_Action"))

            .TSMP = dr("TSMP")
        End With

        Return model
    End Function
    Private Function SerializeInspectionRecordLatest(ByVal dr As DataRow) As InspectionRecordModel
        Dim model As New InspectionRecordModel
        'Inspection Record
        With model
            .InspectionID = FieldValueToString(dr("Inspection_ID"))
            .FileReferenceNo = FieldValueToString(dr("File_Reference_No"))
            .ReferredReferenceNo1 = FieldValueToString(dr("Referred_Reference_No_1"))
            .ReferredReferenceNo2 = FieldValueToString(dr("Referred_Reference_No_2"))
            .ReferredReferenceNo3 = FieldValueToString(dr("Referred_Reference_No_3"))
            '-----------------------------------------------------------------------------'
            'Visit Target
            .SPID = FieldValueToString(dr("SP_ID"))
            .SPEngName = FieldValueToString(dr("SP_Eng_Name"))
            .SPChiName = FieldValueToString(dr("SP_Chi_Name"))
            .SPLastVisitDate = FieldValueToDate(dr("SP_Last_Visit_Date"))
            '-----------------------------------------------------------------------------'
            'Visit Detail
            .VisitDate = FieldValueToDate(dr("Visit_Date"))
            .VisitBeginDtm = FieldValueToDate(dr("Visit_Begin_Dtm"))
            .VisitEndDtm = FieldValueToDate(dr("Visit_End_Dtm"))
            .FormConditionID = FieldValueToString(dr("Form_Condition_ID"))
            .FormConditionValue = FieldValueToString(dr("Form_Condition_Value"))
            .FormConditionRemark = FieldValueToString(dr("Form_Condition_Remark"))
            .MeansOfCommunicationID = FieldValueToString(dr("Means_Of_Communication_ID"))
            .MeansOfCommunicationValue = FieldValueToString(dr("Means_Of_Communication_Value"))
            .MeansOfCommunicationFax = FieldValueToString(dr("Means_Of_Communication_Fax"))
            .MeansOfCommunicationEmail = FieldValueToString(dr("Means_Of_Communication_Email"))
            .Remarks = FieldValueToString(dr("Remarks"))
        End With

        Return model
    End Function

#Region "Format & Convert Function"
    Private Function FieldValueToInt(ByVal value As Object) As Integer
        If IsDBNull(value) Or IsNothing(value) Then
            Return 0
        Else
            Return Convert.ToInt32(value)
        End If
    End Function

    Private Function FieldValueToString(ByVal value As Object) As String
        If IsDBNull(value) Or IsNothing(value) Then
            Return Nothing
        Else
            Return CStr(value).Trim
        End If
    End Function

    Private Function FieldValueToDate(ByVal value As Object) As Date
        If IsDBNull(value) Or IsNothing(value) Then
            Return Date.MinValue
        Else
            Return CDate(value)
        End If
    End Function

    Public Function FormatDateTimeByFormat(dateValue As DateTime, ByVal format As String) As String
        If dateValue <> DateTime.MinValue Then
            Return dateValue.ToString(format)
        End If
        Return ""
    End Function

    Public Function FormatOutputDate(dateValue As Date) As String
        If dateValue <> Date.MinValue Then
            Return dateValue.ToString(DefaultDateOutputFormat)
        End If
        Return ""
    End Function

    Public Function FormatInputDate(dateValue As Date) As String
        If dateValue <> Date.MinValue Then
            Return dateValue.ToString(DefaultDateInputFormat)
        End If
        Return ""
    End Function

    Private Function DateMinvalueToDBNull(ByVal obj As Date) As Object
        If (obj = Date.MinValue) Then
            Return DBNull.Value
        Else
            Return obj
        End If
    End Function

    Public Function ConvertNullableDate(dateVal As Date?) As Date
        If dateVal.HasValue Then
            Return dateVal.Value.Date
        Else
            Return Date.MinValue
        End If
    End Function

    Public Function ConvertDate(strDate As String) As Date
        Try
            If String.IsNullOrEmpty(strDate) Then
                Return Date.MinValue
            Else
                Dim result As Date
                If Date.TryParseExact(strDate, DefaultDateInputFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, result) Then
                    Return result
                End If
            End If
        Catch ex As Exception

        End Try
        Return Date.MinValue
    End Function

    Public Function ConvertStringToInt(ByVal strInt As String, Optional defValue As Integer = 0) As Integer
        If IsNumeric(strInt) Then
            Return CInt(strInt)
        Else
            Return defValue
        End If
    End Function

    Public Function GetTimeFromDate(dateValue As DateTime) As String
        If dateValue <> DateTime.MinValue Then
            Return dateValue.ToString("HH:mm")
        End If
        Return ""
    End Function

    Public Function DataTableToXml(ByVal dt As DataTable, ByVal sName As String) As String
        If dt IsNot Nothing Then
            Dim ms As MemoryStream = Nothing
            Dim XmlWt As XmlTextWriter = Nothing
            ms = New MemoryStream()
            XmlWt = New XmlTextWriter(ms, System.Text.Encoding.Unicode)
            dt.TableName = sName
            dt.WriteXml(XmlWt, XmlWriteMode.IgnoreSchema)
            Dim count As Integer = CInt(ms.Length)
            Dim temp As Byte() = New Byte(count - 1) {}
            ms.Seek(0, SeekOrigin.Begin)
            ms.Read(temp, 0, count)
            Dim ucode As System.Text.UnicodeEncoding = New System.Text.UnicodeEncoding()
            Dim returnValue As String = ucode.GetString(temp).Trim

            If XmlWt IsNot Nothing Then
                XmlWt.Close()
                ms.Close()
                ms.Dispose()
            End If

            Return returnValue
        Else
        End If
        Return ""
    End Function
  
#End Region
End Class
