Imports System.Data.SqlClient
Imports System.Data
Imports Common.Component
Imports Common.DataAccess
Imports Common.ComObject

Namespace BLL
    Public Class MenuBLL
        'Connection
        Private udtDB As Database = New Database()

        Public Property DB() As Database
            Get
                Return udtDB
            End Get
            Set(ByVal Value As Database)
                udtDB = Value
            End Set
        End Property

        Public Function GetMenuItem() As DataTable

            Dim strType As String = "Login"

            Dim dtMenuItem As DataTable
            If HttpContext.Current.Cache.Get("HCSPMenuItem") Is Nothing Then
                dtMenuItem = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_HCSPMenuItem_get_cache", dtMenuItem, dependency)

                db.RunProc("proc_HCSPMenuItem_get_cache", dtMenuItem)
                'CacheHandler.InsertCache("HCSPMenuItem", dtMenuItem, dependency)
                CacheHandler.InsertCache("HCSPMenuItem", dtMenuItem)
            Else
                dtMenuItem = CType(HttpContext.Current.Cache.Get("HCSPMenuItem"), DataTable)
            End If
            Return dtMenuItem


        End Function

        ''' <summary>
        ''' Retrieve the Menu Item for corresponding Service Provider
        ''' </summary>
        ''' <param name="strType">Login (Login Page) / Menu (After Login)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMenuItem(ByVal strType As String, Optional ByVal enumHCSPSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK) As DataTable

            ' Use Application Time, See if need to change to Database Time Later
            'Dim dtmNow As DateTime = DateTime.Now
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim dtmNow As Date = (New Common.ComFunction.GeneralFunction).GetSystemDateTime.Date
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            Dim dtMenuItem As DataTable
            If HttpContext.Current.Cache.Get("HCSPMenuItem") Is Nothing Then
                dtMenuItem = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing

                db.RunProc("proc_HCSPMenuItem_get_cache", dtMenuItem)
                CacheHandler.InsertCache("HCSPMenuItem", dtMenuItem)
            Else
                dtMenuItem = CType(HttpContext.Current.Cache.Get("HCSPMenuItem"), DataTable)
            End If

            Dim dtResult As New DataTable()
            dtResult = dtMenuItem.Clone()

            Dim drNew As DataRow
            Dim drCurrent As DataRow
            Dim i As Integer
            Dim j As Integer
            Dim arrFunctionCode As New ArrayList()

            For i = 0 To dtMenuItem.Rows.Count - 1
                drCurrent = dtMenuItem.Rows(i)

                ' Check Menu Type
                If drCurrent.Item("Type").ToString().Trim().ToUpper() = strType.Trim().ToUpper() Then
                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                    If drCurrent("Available_HCSP_SubPlatform") <> "ALL" AndAlso drCurrent("Available_HCSP_SubPlatform") <> enumHCSPSubPlatform.ToString Then
                        Continue For
                    End If
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                    If Convert.ToDateTime(drCurrent.Item("Effective_date")) <= dtmNow AndAlso _
                        (drCurrent.IsNull("Expiry_Date") OrElse dtmNow < Convert.ToDateTime(drCurrent.Item("Expiry_Date"))) Then

                        If Not arrFunctionCode.Contains(drCurrent.Item("Function_Code").ToString().Trim().ToUpper()) Then
                            ' New Add
                            drNew = dtResult.NewRow()
                            For j = 0 To dtMenuItem.Columns.Count - 1
                                drNew.Item(j) = drCurrent.Item(j)
                            Next
                            dtResult.Rows.Add(drNew)
                            arrFunctionCode.Add(drCurrent.Item("Function_Code").ToString().Trim().ToUpper())
                        Else

                            ' Append the Scheme Code: eg. "HCVS,CIVSS..."
                            ' DataTable Store HCSPMenuItem Order by Display Seq
                            ' Same Function Code should have Same Display Seq and with sequencial Order
                            ' Retrieve the last Entry 
                            dtResult.Rows(dtResult.Rows.Count - 1).Item("Scheme_Code") = dtResult.Rows(dtResult.Rows.Count - 1).Item("Scheme_Code").ToString().Trim().ToUpper() + "," + drCurrent.Item("Scheme_Code").ToString().Trim().ToUpper()
                        End If
                    Else
                        ' Do Nothing: Not Effective Or Expired
                    End If
                Else
                    ' Do Nothing: Not Same Type
                End If
            Next

            'Dim arrFunctionCode As New ArrayList
            'Dim dr As DataRow
            'Dim i As Integer
            'Dim j As Integer
            'For i = 0 To dtMenuItem.Rows.Count - 1
            '    If dtMenuItem.Rows(i).Item("Type") = strType Then
            '        If Not arrFunctionCode.Contains(dtMenuItem.Rows(i).Item("function_code").ToString.Trim) Then
            '            dr = dtResult.NewRow
            '            For j = 0 To dtMenuItem.Columns.Count - 1
            '                dr.Item(j) = dtMenuItem.Rows(i).Item(j)
            '            Next
            '            dtResult.Rows.Add(dr)
            '            arrFunctionCode.Add(dtMenuItem.Rows(i).Item("function_code").ToString.Trim)
            '        Else
            '            dtResult.Rows(dtResult.Rows.Count - 1).Item("scheme_code") = dtResult.Rows(dtResult.Rows.Count - 1).Item("scheme_code").ToString & " " & dtMenuItem.Rows(i).Item("scheme_code")
            '        End If
            '    End If
            'Next

            Return dtResult

        End Function


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Function GetURL(ByVal strFunctionCode As String) As String
            Dim dr() As DataRow = GetMenuItem.Select(String.Format("Function_Code='{0}'", strFunctionCode))
            If dr.Length <> 1 Then Throw New Exception("eHealthAccountDeathRecordMatching.GetURLByFunctionCode: Unexpected no. of rows")
            Return dr(0)("URL")
        End Function

        Public Function GetSystemResourceObjectName_ReturnBtn(ByVal strFunctionCode As String) As String
            ' returns the SystemResourceObjectName_ReturnBtn field value
            Dim dr() As DataRow = GetMenuItem.Select(String.Format("Function_Code='{0}'", strFunctionCode))
            GetMenuItem.Select("Function_Code='" + strFunctionCode + "' AND SystemResourceObjectName_ReturnBtn is Not NULL AND SystemResourceObjectName_ReturnBtn <> ''")
            If dr.Length <> 1 Then Throw New Exception("(HCSP) MenuBLL.SystemResourceObjectName_ReturnBtn (FunctCode=" + strFunctionCode + "): Unexpected no. of rows")
            Return dr(0)("SystemResourceObjectName_ReturnBtn")
        End Function
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
    End Class
End Namespace

