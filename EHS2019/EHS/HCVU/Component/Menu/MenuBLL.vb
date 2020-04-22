Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Component

Namespace Component.Menu
    Public Class MenuBLL

        Public Sub New()

        End Sub

        Public Function GetMenuGroupTable() As DataTable
            Dim dtMenuGroup As DataTable
            If HttpContext.Current.Cache.Get("MenuGroup") Is Nothing Then
                dtMenuGroup = New DataTable
                Dim db As New Database

                db.RunProc("proc_MenuGroup_get_cache", dtMenuGroup)
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

                db.RunProc("proc_MenuItem_get_cache", dtMenuItem)
                CacheHandler.InsertCache("MenuItem", dtMenuItem)
            Else
                dtMenuItem = CType(HttpContext.Current.Cache.Item("MenuItem"), DataTable)
            End If

            Return dtMenuItem
        End Function

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        Public Function GetMenuItemTableBySubPlatform(ByVal enumHCVUSubPlatform As EnumHCVUSubPlatform) As DataTable
            Dim dtMenuItem As DataTable = GetMenuItemTable()

            Dim dtResult As New DataTable()
            dtResult = dtMenuItem.Clone()

            Dim drCurrent As DataRow
            Dim i As Integer

            For i = 0 To dtMenuItem.Rows.Count - 1
                drCurrent = dtMenuItem.Rows(i)

                If drCurrent("Available_HCVU_SubPlatform") <> "ALL" AndAlso drCurrent("Available_HCVU_SubPlatform") <> enumHCVUSubPlatform.ToString Then
                    Continue For
                End If

                dtResult.ImportRow(drCurrent)
            Next

            Return dtResult
        End Function
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

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

