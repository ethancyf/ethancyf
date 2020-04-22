Imports Common.DataAccess
Imports System.Data.SqlClient
Imports HCVU.Component.TaskList
Imports Common.Component.HCVUUser
Imports Common.Component.UserAC
Imports Common.Component.UserRole
Imports HCVU.Component.RoleType
Imports Common.Component


Namespace BLL
    Public Class TaskListControlBLL

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Public Function GetTaskList(ByVal enumHCVUSubPlatform As EnumHCVUSubPlatform) As TaskListModelCollection
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
            Dim dtRoleTypeTaskList As DataTable
            Dim dtTaskList As DataTable

            Dim udtTaskListBLL As New TaskListBLL
            dtRoleTypeTaskList = udtTaskListBLL.GetRoleTypeTaskList
            dtTaskList = udtTaskListBLL.GetAllTaskList

            Dim udtTaskListCollection As New TaskListModelCollection
            Dim udtTaskList As TaskListModel

            Dim drRoleTypeTaskList() As DataRow
            Dim drTaskList() As DataRow
            Dim strTaskList_ID As String
            Dim dr As DataRow

            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
            Dim udtUserRole As UserRoleModel

            Dim udtRoleTypeBLL As New RoleTypeBLL
            Dim dtRoleType As DataTable
            Dim drRoleType As DataRow()

            For Each udtUserRole In udtHCVUUser.UserRoleCollection.Values

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                ' Check User Role Type which is available in SubPlatform
                dtRoleType = udtRoleTypeBLL.GetRoleTypeTable()

                drRoleType = dtRoleType.Select("(Available_HCVU_SubPlatform = 'ALL' OR Available_HCVU_SubPlatform = '" + enumHCVUSubPlatform.ToString + "')" _
                                                                 + "AND Role_Type = '" & udtUserRole.RoleType & "'")

                If drRoleType.Length = 0 Then Continue For
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

                drRoleTypeTaskList = dtRoleTypeTaskList.Select("Role_Type = '" & udtUserRole.RoleType & "'")
                For Each dr In drRoleTypeTaskList
                    strTaskList_ID = dr.Item("TaskList_ID")
                    If udtTaskListCollection.Item(strTaskList_ID) Is Nothing Then
                        drTaskList = dtTaskList.Select("TaskList_ID = '" & strTaskList_ID & "'")
                        If drTaskList.Length = 1 Then
                            udtTaskList = New TaskListModel
                            udtTaskList.TaskListID = strTaskList_ID
                            'udtTaskList.Title = drTaskList(0).Item("Title")
                            'udtTaskList.TaskDescription = drTaskList(0).Item("Task")
                            udtTaskList.Title = HttpContext.GetGlobalResourceObject("Text", strTaskList_ID & "Title").ToString()
                            udtTaskList.TaskDescription = HttpContext.GetGlobalResourceObject("Text", strTaskList_ID & "Task").ToString()
                            udtTaskList.URL = drTaskList(0).Item("URL")
                            udtTaskList.DisplaySeq = drTaskList(0).Item("Display_Seq")
                            udtTaskListCollection.Add(udtTaskList)
                        End If
                    End If
                Next
            Next

            udtTaskListCollection.Sort()
            Return udtTaskListCollection
        End Function

    End Class
End Namespace

