Imports Common.Component.AccessRight
Imports Common.Component.HCVUUser
Imports HCVU.Component.Menu
Imports System.Threading
Imports Common.Component

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

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        Public Sub BuildMenu(ByVal ulMenu As HtmlGenericControl, ByVal enumHCVUSubPlatform As EnumHCVUSubPlatform, ByVal blnHideDisabledMenuItem As Boolean)
            Dim udtAccessRightCollection As AccessRightModelCollection = (New HCVUUserBLL).GetHCVUUser().AccessRightCollection

            Dim udtMenuBLL As New MenuBLL

            Dim dtMenuGroup As DataTable = udtMenuBLL.GetMenuGroupTable()
            Dim dtMenuItem As DataTable = udtMenuBLL.GetMenuItemTableBySubPlatform(enumHCVUSubPlatform)

            For Each dr As DataRow In dtMenuGroup.Rows
                Dim objEffectiveDate As Object = dr("Effective_Date")

                If IsDBNull(objEffectiveDate) OrElse DateTime.Now > CType(objEffectiveDate, DateTime) Then
                    Dim strGroupName As String = dr("Group_Name").ToString.Trim

                    ' Get the parent (Level 1)
                    Dim liParent As HtmlGenericControl = CreateParent(GetMenuGroupDesc(strGroupName, dtMenuGroup))

                    ' Retrieve the child of the parent (Level 2 & 3)
                    PopulateSubItem(dtMenuItem, strGroupName, liParent, udtAccessRightCollection, blnHideDisabledMenuItem)

                    ' Add the parent to the menu
                    If liParent.Controls.Count > 1 Then
                        ulMenu.Controls.Add(liParent)
                    End If

                End If

            Next
        End Sub
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        '

        Private Sub PopulateSubItem(ByVal dtMenuItem As DataTable, ByVal strGroupName As String, ByVal miParent As HtmlGenericControl, ByVal udtAccessRightCollection As AccessRightModelCollection, ByVal blnHideDisabledMenuItem As Boolean)
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

                                ' Replace LinkButton by HtmlGenericControl
                                Dim anchor As LinkButton = CType(liTarget.Controls.Item(0), LinkButton)
                                Dim anchor2 As New HtmlGenericControl("a")

                                anchor2.Attributes("class") = anchor.Attributes("class")
                                anchor2.InnerText = anchor.Text
                                liTarget.Controls.Remove(anchor)

                                ' Add an arrow icon the target parent
                                Dim img As New HtmlGenericControl("img")
                                img.Attributes.Add("src", "../Images/menu/arrow.gif")
                                img.Style.Add("padding-right", "2px")
                                img.Style.Add("padding-top", "4px")
                                img.Style.Add("float", "right")

                                anchor2.Style.Add("padding-right", "0")
                                anchor2.Controls.Add(img)

                                liTarget.Controls.AddAt(0, anchor2)

                            Else
                                Dim ulTarget As HtmlGenericControl = CType(liTarget.Controls.Item(1), HtmlGenericControl)
                                ulTarget.Controls.Add(liChild)
                            End If

                            ' Add the item to the dictionary
                            dicMenuItem.Add(CInt(dr("Item_ID")), liChild)

                        End If

                    End If

                Else
                    ' Display_Seq is 0, the parent item is the only item, set the URL for it without arrow
                    Dim anchor As HtmlGenericControl = CType(miParent.Controls.Item(0), HtmlGenericControl)
                    Dim anchor2 As New LinkButton

                    anchor2.Attributes("class") = anchor.Attributes("class")
                    anchor2.Text = anchor.Attributes("text")

                    If udtAccessRightCollection.Item(dr.Item("Function_Code")).Allow Then
                        miParent.Attributes.Add("class", "TopMenuItem")

                        Dim link As String = System.Web.VirtualPathUtility.ToAbsolute(dr("URL").ToString.Trim)

                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        ' ------------------------------------------------------------------------
                        'anchor.Attributes.Add("onclick", "location.href='" & link & "';closeMenu();return false;")
                        anchor2.CommandArgument = link

                        AddHandler anchor2.Click, AddressOf lbtnMenuItem_Click
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                    Else
                        miParent.Attributes.Add("class", "DisabledTopMenuItem")
                        anchor2.Attributes.Add("onclick", "return false;")
                    End If

                    anchor2.Style.Add("padding-right", "5px")

                    miParent.Controls.Remove(anchor)
                    miParent.Controls.AddAt(0, anchor2)
                End If
            Next

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
            ' Remove menu item that cannot be access 
            If blnHideDisabledMenuItem = True Then
                For Each liTarget As HtmlGenericControl In dicMenuItem.Values
                    If liTarget.Attributes("class") <> "DefaultMenuItem" Then
                        liTarget.Parent.Controls.Remove(liTarget)
                    End If
                Next
            End If
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            'Add list to parent
            If ulChild.HasControls Then
                miParent.Controls.Add(ulChild)
            End If

        End Sub

        Private Function CreateChild(ByVal strFunctionCode As String, ByVal strDescription As String, ByVal Url As String, ByVal udtAccessRightCollection As AccessRightModelCollection, ByVal strLevel As String) As HtmlGenericControl
            Dim menuItem As New HtmlGenericControl("li")
            Dim anchor As New LinkButton
            anchor.Attributes.Add("class", strLevel)

            anchor.Text = strDescription

            If udtAccessRightCollection.Item(strFunctionCode).Allow Then
                menuItem.Attributes.Add("class", "DefaultMenuItem")

                Dim link As String = System.Web.VirtualPathUtility.ToAbsolute(Url)
                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                'anchor.Attributes.Add("onclick", "location.href='" & link & "';closeMenu();return false;")
                anchor.CommandArgument = link

                AddHandler anchor.Click, AddressOf lbtnMenuItem_Click
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Else
                menuItem.Attributes.Add("class", "DisabledMenuItem")
                anchor.Attributes.Add("onclick", "return false;")
            End If

            menuItem.Controls.Add(anchor)

            Return menuItem

        End Function

        Private Function CreateParent(ByVal strDescription As String) As HtmlGenericControl
            Dim menuItem As New HtmlGenericControl("li")
            Dim anchor As New HtmlGenericControl("a")

            anchor.Attributes.Add("class", "level1")
            anchor.Attributes.Add("text", strDescription)
            anchor.InnerText = strDescription
            Dim img As New HtmlGenericControl("img")
            img.Attributes.Add("src", "../Images/menu/arrow.gif")
            img.Style.Add("padding-left", "5px")
            anchor.Controls.Add(img)
            menuItem.Controls.Add(anchor)
            menuItem.Attributes.Add("class", "TopMenuItem")
            Return menuItem
        End Function

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        Protected Sub lbtnMenuItem_Click(sender As Object, e As EventArgs)
            ' Add page key to URL after menu click
            RedirectHandler.ToURL(DirectCast(sender, LinkButton).CommandArgument)
        End Sub
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Class

End Namespace
