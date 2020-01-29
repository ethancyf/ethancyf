Imports Common.DataAccess
Imports System.Data.SqlClient
Imports HCVU.Component.TaskList
Imports Common.Component.HCVUUser
Imports Common.Component.UserAC
Imports Common.Component.UserRole


Namespace BLL
    Public Class TaskListControlBLL

        Public Function GetTaskList() As TaskListModelCollection
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

            For Each udtUserRole In udtHCVUUser.UserRoleCollection.Values
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

