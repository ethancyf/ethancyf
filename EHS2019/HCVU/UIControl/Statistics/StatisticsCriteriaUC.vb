Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.StaticData
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Public MustInherit Class StatisticsCriteriaUC
    Inherits UserControl
    Implements IStatisticsCriteriaUC

    ' <FID></FID>
    ' <DescResource></DescResource>
    ' <Visible></Visible>
    ' <DefaultValue></DefaultValue>
    ' <SPParamName></SPParamName>

    Public Class FieldSetting
        Public Const FID As String = "FID"
        Public Const DescResource As String = "DescResource"
        Public Const Visible As String = "Visible"
        Public Const DefaultValue As String = "DefaultValue"
        Public Const SPParamName As String = "SPParamName"
        Public Const AllowDH As String = "AllowDH"
    End Class

    Public Class PeriodBreakDown
        Public Const Daily As Char = "D"
        Public Const Monthly As Char = "M"
        Public Const Yearly As Char = "Y"
    End Class

    Public Class Condition
        Public Const YES As String = "Y"
        Public Const NO As String = "N"
    End Class

    Public Class TypeOfCountingItem
        Public Const Transaction As String = "T"
        Public Const Subsidy As String = "S"
    End Class

    Public Class TypeOfBreakDown
        Public Const Profession As String = "P"
        Public Const District As String = "D"
    End Class

    Public Class LegendType
        Public Const District As String = "District"
        Public Const Profession As String = "Profession"
    End Class

    Public Enum MultiSelectionTypeEnum
        NoSelection = -1
        Any = 0
        Specific = 1
    End Enum

    Protected Const START_YEAR As Integer = 2009
    Protected Const NO_SELECTION As Integer = -1
    Protected Const DROP_DOWN_EMPTY As String = ""
    Protected Const FUNCTION_CODE As String = "010703"
    Protected Const CUT_OFF_DATE_DATATYPE_ID As String = "dbEVS_Enquiry"

    Private Const AllowDHValue As String = "DH"

    ' Format: (FID, <Field></Field>)
    Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

    ' Get <Field> Field List </Field>
    Public Function GetSetting(ByVal strCode As String) As Dictionary(Of String, String)
        If IsExistValue(strCode) Then
            Return _dicSetting(strCode)
        Else
            Return Nothing
        End If
    End Function

    ' Get <FieldName> Value </FieldName>
    Public Function GetSetting(ByVal strCode As String, ByVal strFieldCode As String) As String
        ' Check Setting
        If IsExistValue(strCode) Then
            Dim dicFieldSetting As New Dictionary(Of String, String)
            dicFieldSetting = _dicSetting(strCode)

            ' Check Field
            If dicFieldSetting.ContainsKey(strFieldCode) Then
                Return dicFieldSetting(strFieldCode)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function IsExistValue(ByVal strCode As String) As Boolean
        Return _dicSetting.ContainsKey(strCode)
    End Function

    Public Function IsExistValue(ByVal strCode As String, ByVal strFieldCode As String) As Boolean
        ' Check Setting
        If IsExistValue(strCode) Then
            Dim dicFieldSetting As New Dictionary(Of String, String)
            dicFieldSetting = _dicSetting(strCode)

            ' Check Field
            Return dicFieldSetting.ContainsKey(strFieldCode)
        Else
            Return False
        End If
    End Function

    ' Build control method, pass setting to build
    Public Overridable Sub Build(ByVal dicSetting As System.Collections.Generic.Dictionary(Of String, System.Collections.Generic.Dictionary(Of String, String))) Implements IStatisticsCriteriaUC.Build
        _dicSetting = dicSetting
        InitControl()
    End Sub

    ' Override in child class, set control visibility and description
    Public Overridable Sub InitControl()
    End Sub

    ' Validation of dynamic control, exception case
    Public MustOverride Sub ValidateCriteriaInput(ByVal strStatisticsID As String, ByRef lstError As System.Collections.Generic.List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As System.Collections.Generic.List(Of String), ByRef lstErrorParam2 As System.Collections.Generic.List(Of String)) Implements IStatisticsCriteriaUC.ValidateCriteriaInput

    ' Validation of dynamic control
    Public MustOverride Function ValidateCriteriaInput(ByVal strStatisticsID As String) As System.Collections.Generic.List(Of Common.ComObject.SystemMessage) Implements IStatisticsCriteriaUC.ValidateCriteriaInput

    ' Get dynamic control parameter (one string), for audit log
    Public MustOverride Function GetParameterString() As Common.ComFunction.ParameterFunction.ParameterCollection Implements IStatisticsCriteriaUC.GetParameterString

    ' Get dynamic control parameter and show on the result page
    Public MustOverride Function GetParameterList() As Common.ComFunction.ParameterFunction.ParameterCollection Implements IStatisticsCriteriaUC.GetParameterList

    ' Get dynamic control criteria input for execute stored procedure
    Public MustOverride Function GetCriteriaInput() As Common.ComFunction.ParameterFunction.StoreProcParamCollection Implements IStatisticsCriteriaUC.GetCriteriaInput

    Public MustOverride Sub SetErrorComponentVisibility(blnVisible As Boolean) Implements IStatisticsCriteriaUC.SetErrorComponentVisibility

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Overridable Function GetReportGenerationDate() As DateTime? Implements IStatisticsCriteriaUC.GetReportGenerationDate
        Return Nothing
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

#Region "Build control common functions"

    ' Period break down (Static data is ready)
    Protected Overridable Sub BuildPeriodBreakDownComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("PeriodBreakDown")
        ddlComponent.DataSource = udtStaticDataList
        ddlComponent.DataValueField = "ItemNo"
        ddlComponent.DataTextField = "DataValue"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0
    End Sub

    ' Scheme
    Protected Overridable Sub BuildSchemeComponent(ByVal ddlComponent As DropDownList)
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtUserRoleBLL As New UserRoleBLL

        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

        For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
            For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                    If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                End If
            Next
        Next

        ddlComponent.Items.Clear()

        ddlComponent.DataSource = udtSchemeClaimModelListFilter
        ddlComponent.DataTextField = "DisplayCode"
        ddlComponent.DataValueField = "SchemeCode"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0
    End Sub

    ' Type of count (Static data is ready)
    Protected Overridable Sub BuildTypeOfCountComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("TypeOfCount")
        ddlComponent.DataSource = udtStaticDataList
        ddlComponent.DataValueField = "ItemNo"
        ddlComponent.DataTextField = "DataValue"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0
    End Sub

    ' Subsidy
    Protected Overridable Sub BuildSubsidyComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0
    End Sub

    ' Subsidy (Scheme + Subsidy relation)
    Protected Sub ResetSubsidySelection(ByVal ddlSchemeComponent As DropDownList, ByVal ddlSubsidyComponent As DropDownList)
        ddlSubsidyComponent.Items.Clear()

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimWithSubsidyGroupList As SchemeClaimModelCollection

        udtSchemeClaimWithSubsidyGroupList = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup()

        ' CRE16-002 Revamp VSS [Start][Winnie]
        Dim dtSubsidizeCode As New DataTable
        dtSubsidizeCode.Columns.Add("SubsidizeDisplayCode")
        dtSubsidizeCode.Columns.Add("DataValue")

        Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimWithSubsidyGroupList.Filter(ddlSchemeComponent.SelectedValue)

        If Not udtSchemeClaimModel Is Nothing Then

            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

                Dim drSubsidizeCode As DataRow

                ' Add record if record status = A (Available)
                If udtSubsidizeGroupClaimModel.RecordStatus = "A" Then
                    drSubsidizeCode = dtSubsidizeCode.NewRow

                    ' For HCVS (Scheme code) > EHCV (Subsidize code), change display to Voucher
                    If ddlSchemeComponent.SelectedValue.Trim = "HCVS" AndAlso udtSubsidizeGroupClaimModel.SubsidizeDisplayCode = "EHCV" Then
                        drSubsidizeCode("SubsidizeDisplayCode") = "Voucher"
                    Else
                        drSubsidizeCode("SubsidizeDisplayCode") = udtSubsidizeGroupClaimModel.SubsidizeDisplayCode
                    End If
                    drSubsidizeCode("DataValue") = udtSubsidizeGroupClaimModel.SubsidizeDisplayCode

                    dtSubsidizeCode.Rows.Add(drSubsidizeCode)
                End If
            Next

            Dim dvSubsidizeCode As New DataView(dtSubsidizeCode)
            dvSubsidizeCode.Sort = "SubsidizeDisplayCode ASC"

            ddlSubsidyComponent.DataSource = dvSubsidizeCode.ToTable(True)
            ddlSubsidyComponent.DataTextField = "SubsidizeDisplayCode"
            ddlSubsidyComponent.DataValueField = "DataValue"
            ddlSubsidyComponent.DataBind()
        End If
        ' CRE16-002 Revamp VSS [End][Winnie]

        ddlSubsidyComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlSubsidyComponent.SelectedIndex = 0

    End Sub

    ' Type of break down (Static data is ready)
    Protected Overridable Sub BuildTypeOfBreakDownComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("BreakDownType")
        ddlComponent.DataSource = udtStaticDataList
        ddlComponent.DataValueField = "ItemNo"
        ddlComponent.DataTextField = "DataValue"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0
    End Sub

    ' District
    Protected Overridable Sub BuildDistrictComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        cboxListComponent.Items.Clear()

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'Dim dtSDDistrictList As DataTable = (New StatisticsBLL).GetSDDistrictAll
        Dim dtSDDistrictList As DataTable = (New StatisticsBLL).GetDistrictBoardList()
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        For Each row As DataRow In dtSDDistrictList.Rows
            strItemText = row.Item("district_board").ToString.Trim + " (" + row.Item("district_board_shortname_SD").ToString.Trim + ")"
            strItemValue = row.Item("district_board_shortname_SD").ToString.Trim
            cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
        Next

        ' Allow DH
        AddDHCapitalToCheckboxComponent(cboxListComponent, strFieldID)

    End Sub

    ' Means of input
    Protected Overridable Sub BuildMeansOfInputComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("MeansOfInput_STSU")
        ddlComponent.DataSource = udtStaticDataList
        ddlComponent.DataValueField = "ItemNo"
        ddlComponent.DataTextField = "DataValue"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0

    End Sub

    ' Reason for visit (Static data is ready)
    Protected Overridable Sub BuildReasonForVisitComponent(ByVal ddlComponent As DropDownList)
        ddlComponent.Items.Clear()

        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("ReasonForVisit")
        ddlComponent.DataSource = udtStaticDataList
        ddlComponent.DataValueField = "ItemNo"
        ddlComponent.DataTextField = "DataValue"
        ddlComponent.DataBind()

        ddlComponent.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), DROP_DOWN_EMPTY))

        ddlComponent.SelectedIndex = 0

    End Sub

    ' Submission Method
    Protected Overridable Sub BuildSubmissionMethodComponent(ByVal cboxListComponent As CheckBoxList)
        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("SubmissionMethod")

        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        cboxListComponent.Items.Clear()

        For Each staticDataModel As StaticDataModel In udtStaticDataList
            strItemText = staticDataModel.DataValue.ToString.Trim
            strItemValue = staticDataModel.ItemNo.ToString.Trim
            cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
        Next

    End Sub

    ' Profession
    Protected Overridable Sub BuildProfessionComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        Dim udtProfessionModelCollection As ProfessionModelCollection

        udtProfessionModelCollection = ProfessionBLL.GetProfessionList()

        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        cboxListComponent.Items.Clear()

        For Each profModel As ProfessionModel In udtProfessionModelCollection
            strItemText = profModel.ServiceCategoryDesc.ToString.Trim + " (" + profModel.ServiceCategoryCode.ToString.Trim + ")"
            strItemValue = profModel.ServiceCategoryCode.ToString.Trim
            cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
        Next

        ' Allow DH
        AddDHToCheckboxComponent(cboxListComponent, strFieldID)

    End Sub

    ' Profession (Scheme + Profession relation)
    Protected Overridable Sub BuildProfessionComponent(ByVal ddlComponent As DropDownList, ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        Dim udtProfessionModelCollection As ProfessionModelCollection
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList()

        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection
        Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL()
        udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache()

        cboxListComponent.Items.Clear()

        If ddlComponent.SelectedIndex > 0 Then

            If Not udtSchemeBackOfficeList Is Nothing Then

                For Each udtSchemeBackOffice As SchemeBackOfficeModel In udtSchemeBackOfficeList

                    ' CRE17-014 (Enhance eHSU0002) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Find item by model> display code instead of scheme_code to handle EVSS
                    If udtSchemeBackOffice.DisplayCode.Trim.Equals(ddlComponent.SelectedItem.Text.Trim()) Then
                        For Each profModel As ProfessionModel In udtProfessionModelCollection
                            If udtSchemeBackOffice.EligibleProfesional(profModel.ServiceCategoryCode.Trim) Then
                                strItemText = profModel.ServiceCategoryDesc.ToString.Trim + " (" + profModel.ServiceCategoryCode.ToString.Trim + ")"
                                strItemValue = profModel.ServiceCategoryCode.ToString.Trim
                                cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
                            End If
                        Next
                    End If
                    ' CRE17-014 (Enhance eHSU0002) [End][Winnie]
                Next

            End If

        End If

        ' Allow DH
        AddDHToCheckboxComponent(cboxListComponent, strFieldID)

    End Sub

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    ' Scheme (Multiple Selection)
    Protected Overridable Sub BuildSchemeMultipleComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtUserRoleBLL As New UserRoleBLL

        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

        For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
            For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                    If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                End If
            Next
        Next

        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        cboxListComponent.Items.Clear()

        If Not udtSchemeClaimModelListFilter Is Nothing Then

            For Each udtSchemeC As SchemeClaimModel In udtSchemeClaimModelListFilter
                strItemText = udtSchemeC.DisplayCode
                strItemValue = udtSchemeC.SchemeCode
                cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))

            Next
        End If

    End Sub
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    ' Transaction Status (Multiple Selection)
    Protected Overridable Sub BuildTransStatusMultipleComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)

        Dim dtTransStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("HCVUClaimTransManagementStatus")

        Dim strItemText As String = String.Empty
        Dim strItemValue As String = String.Empty

        cboxListComponent.Items.Clear()

        If Not dtTransStatus Is Nothing AndAlso dtTransStatus.Rows.Count > 0 Then
            For Each dr As DataRow In dtTransStatus.Rows
                strItemText = dr.Item("Status_Description")
                strItemValue = dr.Item("Status_Value")
                cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
            Next
        End If

    End Sub
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]


    ' Allow DH
    Protected Sub AddDHToCheckboxComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)

        If IsExistValue(strFieldID, FieldSetting.AllowDH) Then
            If GetSetting(strFieldID, FieldSetting.AllowDH) = Condition.YES Then
                Dim strItemText As String = String.Empty
                Dim strItemValue As String = String.Empty
                Dim strDepartmentOfHealth As String = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")

                strItemText = strDepartmentOfHealth + " (" + AllowDHValue + ")"
                strItemValue = AllowDHValue
                cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
            End If
        End If

    End Sub

    ' Allow DH (Capital letter)
    Protected Sub AddDHCapitalToCheckboxComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)

        If IsExistValue(strFieldID, FieldSetting.AllowDH) Then
            If GetSetting(strFieldID, FieldSetting.AllowDH) = Condition.YES Then
                Dim strItemText As String = String.Empty
                Dim strItemValue As String = String.Empty
                Dim strDepartmentOfHealth As String = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")

                strItemText = strDepartmentOfHealth.ToUpper + " (" + AllowDHValue + ")"
                strItemValue = AllowDHValue
                cboxListComponent.Items.Add(New ListItem(strItemText, strItemValue))
            End If
        End If

    End Sub


#End Region

End Class
