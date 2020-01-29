Imports Common.DataAccess
Imports Common.ComObject

Namespace Component.Menu
    Public Class MenuBLL

        Public Sub New()

        End Sub

        Public Function GetMenuGroupTable() As DataTable
            Dim dtMenuGroup As DataTable
            If HttpContext.Current.Cache.Get("MenuGroup") Is Nothing Then
                dtMenuGroup = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                db.RunProc("proc_MenuGroup_get_cache", dtMenuGroup)
                'db.RunProc("proc_MenuGroup_get_cache", dtMenuGroup, dependency)
                'CacheHandler.InsertCache("MenuGroup", dtMenuGroup, dependency)
                CacheHandler.InsertCache("MenuGroup", dtMenuGroup)
            Else
                dtMenuGroup = CType(HttpContext.Current.Cache.Get("MenuGroup"), DataTable)
            End If
            Return dtMenuGroup
        End Function

        Public Function GetMenuItemTable() As DataTable
            Dim dtMenuItem As DataTable
            If HttpContext.Current.Cache.Item("MenuItem") Is Nothing Then
                dtMenuItem = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_MenuItem_get_cache", dtMenuItem, dependency)
                'CacheHandler.InsertCache("MenuItem", dtMenuItem, dependency)
                db.RunProc("proc_MenuItem_get_cache", dtMenuItem)
                CacheHandler.InsertCache("MenuItem", dtMenuItem)
            Else
                dtMenuItem = CType(HttpContext.Current.Cache.Item("MenuItem"), DataTable)
            End If
            Return dtMenuItem
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
            Dim drs() As DataRow = GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode.Trim))
            If drs.Length > 0 Then
                Return drs(0)("URL")
            Else
                Throw New Exception(String.Format("Error: Class = [Common.Component.Menu.MenuBll], Method = [GetURLByFunctionCode], Message = The function code({0}) is not exist", strFunctionCode))
            End If
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]

    End Class
End Namespace

