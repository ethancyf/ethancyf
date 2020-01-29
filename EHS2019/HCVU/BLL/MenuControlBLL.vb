Imports Common.Component.AccessRight
Imports Common.Component.HCVUUser
Imports HCVU.Component.Menu
Imports System.Threading

Namespace BLL

    Public Class MenuControlBLL

        Public Sub New()
        End Sub

        Private Function GetMenuGroupDesc(ByVal strGroupName As String, ByVal dt As DataTable) As String
            Dim dr As DataRow() = dt.Select(String.Format("Group_Name = '{0}'", strGroupName))

            If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
                Return dr(0)("Chinese_Description").ToString.Trim
            Else
                Return dr(0)("Description").ToString.Trim
            End If

        End Function

        Private Function GetMenuItemDesc(ByVal strItemName As String, ByRef dt As DataTable) As String
            Dim dr As DataRow() = dt.Select(String.Format("Item_Name = '{0}'", strItemName))

            If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
                Return dr(0)("Chinese_Description").ToString.Trim
            Else
                Return dr(0)("Description").ToString.Trim
            End If

        End Function        

        Public Sub BuildMenu(ByVal ulMenu As HtmlGenericControl)
            Dim udtAccessRightCollection As AccessRightModelCollection = (New HCVUUserBLL).GetHCVUUser().AccessRightCollection

            Dim udtMenuBLL As New MenuBLL

            Dim dtMenuGroup As DataTable = udtMenuBLL.GetMenuGroupTable
            Dim dtMenuItem As DataTable = udtMenuBLL.GetMenuItemTable

            For Each dr As DataRow In dtMenuGroup.Rows
                Dim objEffectiveDate As Object = dr("Effective_Date")

                If IsDBNull(objEffectiveDate) OrElse DateTime.Now > CType(objEffectiveDate, DateTime) Then
                    Dim strGroupName As String = dr("Group_Name").ToString.Trim

                    ' Get the parent
                    Dim liParent As HtmlGenericControl = CreateParent(GetMenuGroupDesc(strGroupName, dtMenuGroup))

                    ' Retrieve the child of the parent
                    PopulateSubItem(dtMenuItem, strGroupName, liParent, udtAccessRightCollection)

                    ' Add the parent to the menu
                    ulMenu.Controls.Add(liParent)

                End If

            Next

        End Sub

        '

        Private Sub PopulateSubItem(ByVal dtMenuItem As DataTable, ByVal strGroupName As String, ByVal miParent As HtmlGenericControl, ByVal udtAccessRightCollection As AccessRightModelCollection)
            Dim dicMenuItem As New Dictionary(Of String, HtmlGenericControl)

            Dim ulChild As New HtmlGenericControl("ul")
            ulChild.Attributes.Add("class", "level2")

            For Each dr As DataRow In dtMenuItem.Select(String.Format("Group_Name = '{0}'", strGroupName), "Display_Seq")
                If CInt(dr("Display_Seq")) <> 0 Then
                    ' Display_Seq is not 0, add the current item to its parent item

                    Dim objEffectiveDate As Object = dr("Effective_Date")

                    If IsDBNull(objEffectiveDate) OrElse DateTime.Now > CType(objEffectiveDate, DateTime) Then
                        If IsDBNull(dr("Parent_Item_ID")) Then
                            'Build Level 2 Submenu
                            Dim liChild As HtmlGenericControl = CreateChild(dr("Function_Code").ToString.Trim, _
                                                        GetMenuItemDesc(dr("Item_Name").ToString.Trim, dtMenuItem), _
                                                        dr("URL").ToString.Trim, udtAccessRightCollection, "level2")


                            ulChild.Controls.Add(liChild)

                            ' Add the item to the dictionary
                            dicMenuItem.Add(CStr(dr("Item_ID")), liChild)

                        Else
                            'Build Level 3 Submenu

                            ' Find the target parent item
                            Dim liTarget As HtmlGenericControl = dicMenuItem(CStr(dr("Parent_Item_ID")))

                            ' Create the child
                            Dim liChild As HtmlGenericControl = CreateChild(dr("Function_Code").ToString.Trim, _
                                                        GetMenuItemDesc(dr("Item_Name").ToString.Trim, dtMenuItem), _
                                                        dr("URL").ToString.Trim, udtAccessRightCollection, "level3")


                            ' Set the target parent to be accessible if the child is accessible
                            If udtAccessRightCollection.Item(dr.Item("Function_Code")).Allow Then
                                liTarget.Attributes.Add("class", "DefaultMenuItem")
                            End If


                            ' Add the child                            
                            If liTarget.Controls.Count = 1 Then
                                Dim ulTarget As New HtmlGenericControl("ul")
                                ulTarget.Attributes.Add("class", "level3")
                                ulTarget.Controls.Add(liChild)
                                liTarget.Controls.Add(ulTarget)

                                ' Add an arrow icon the target parent
                                Dim anchor As HtmlGenericControl = CType(liTarget.Controls.Item(0), HtmlGenericControl)
                                Dim img As New HtmlGenericControl("img")
                                img.Attributes.Add("src", "../Images/menu/arrow.gif")
                                img.Style.Add("padding-right", "2px")
                                img.Style.Add("padding-top", "4px")
                                img.Style.Add("float", "right")
                                anchor.Style.Add("padding-right", "0")
                                anchor.Controls.Add(img)
                            Else
                                Dim ulTarget As HtmlGenericControl = CType(liTarget.Controls.Item(1), HtmlGenericControl)
                                ulTarget.Controls.Add(liChild)
                            End If

                            ' Add the item to the dictionary
                            dicMenuItem.Add(CInt(dr("Item_ID")), liChild)

                        End If

                    End If

                Else
                    ' Display_Seq is 0, the parent item is the only item, set the URL for it
                    Dim anchor As HtmlGenericControl = CType(miParent.Controls.Item(0), HtmlGenericControl)

                    If udtAccessRightCollection.Item(dr.Item("Function_Code")).Allow Then
                        miParent.Attributes.Add("class", "TopMenuItem")

                        Dim link As String = System.Web.VirtualPathUtility.ToAbsolute(dr("URL").ToString.Trim)
                        anchor.Attributes.Add("onclick", "location.href='" & link & "';closeMenu();return false;")

                    Else
                        miParent.Attributes.Add("class", "DisabledTopMenuItem")
                    End If

                    'remove arrow from parent
                    anchor.Style.Add("padding-right", "5px")
                    anchor.Controls.RemoveAt(1)
                End If
            Next

            'Add list to parent
            If ulChild.HasControls Then
                miParent.Controls.Add(ulChild)
            End If

        End Sub

        Private Function CreateChild(ByVal strFunctionCode As String, ByVal strDescription As String, ByVal Url As String, ByVal udtAccessRightCollection As AccessRightModelCollection, ByVal strLevel As String) As HtmlGenericControl
            Dim menuItem As New HtmlGenericControl("li")
            Dim anchor As New HtmlGenericControl("a")
            anchor.Attributes.Add("class", strLevel)

            anchor.InnerText = strDescription

            If udtAccessRightCollection.Item(strFunctionCode).Allow Then
                menuItem.Attributes.Add("class", "DefaultMenuItem")

                Dim link As String = System.Web.VirtualPathUtility.ToAbsolute(Url)
                anchor.Attributes.Add("onclick", "location.href='" & link & "';closeMenu();return false;")
            Else
                menuItem.Attributes.Add("class", "DisabledMenuItem")
            End If

            menuItem.Controls.Add(anchor)

            Return menuItem

        End Function

        Private Function CreateParent(ByVal strDescription As String) As HtmlGenericControl
            Dim menuItem As New HtmlGenericControl("li")
            Dim anchor As New HtmlGenericControl("a")

            anchor.Attributes.Add("class", "level1")
            anchor.InnerText = strDescription
            Dim img As New HtmlGenericControl("img")
            img.Attributes.Add("src", "../Images/menu/arrow.gif")
            img.Style.Add("padding-left", "5px")
            anchor.Controls.Add(img)
            menuItem.Controls.Add(anchor)            
            menuItem.Attributes.Add("class", "TopMenuItem")
            Return menuItem
        End Function

    End Class

End Namespace
