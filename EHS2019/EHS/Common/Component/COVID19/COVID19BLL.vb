Imports Common.Component
Imports Common.DataAccess
Imports Common.ComObject
Imports System.Data.SqlClient
Imports System.Data
Imports System.Linq
Imports System.Web.Script.Serialization
Imports Common.ComFunction
Imports System.IO

Namespace Component.COVID19
    Public Class COVID19BLL

#Region "Constant"
        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_COVID19VaccineBrand As String = "COVID19BLL_ALL_COVID19VaccineBrand"
            Public Const CACHE_ALL_COVID19VaccineBrandLotDetail As String = "COVID19BLL_ALL_COVID19VaccineBrandLotDetail"
            Public Const CACHE_ALL_COVID19VaccineLotMapping As String = "COVID19BLL_ALL_COVID19VaccineLotMapping"
            Public Const CACHE_ALL_VaccineCentreSPMapping As String = "COVID19BLL_ALL_VaccineCentreSPMapping"
        End Class
#End Region

#Region "Private Member"
        Dim udcValidator As New Common.Validation.Validator
        'Connection
        Private udtDB As Database = New Database()
#End Region

#Region "Property"
        Public Property DB() As Database
            Get
                Return udtDB
            End Get
            Set(ByVal Value As Database)
                udtDB = Value
            End Set
        End Property

#End Region

#Region "Cache"
        Public Function GetCOVID19VaccineBrand() As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrand)) Then

                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrand), DataTable)

            Else

                Try
                    db.RunProc("proc_COVID19VaccineBrand_get_all_cache", dt)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrand, dt)
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

        Public Function GetCOVID19VaccineBrandLotDetail() As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrandLotDetail)) Then
                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrandLotDetail), DataTable)

            Else
                Try
                    db.RunProc("proc_COVID19VaccineBrandLotDetail_getAll", dt)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineBrandLotDetail, dt)
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

        Public Function GetCOVID19VaccineLotMapping() As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineLotMapping)) Then

                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineLotMapping), DataTable)

            Else

                Try
                    db.RunProc("proc_COVID19VaccineLotMapping_getAll", dt)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_COVID19VaccineLotMapping, dt)
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

        Public Function GetVaccineCentreSPMapping() As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCentreSPMapping)) Then

                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCentreSPMapping), DataTable)

            Else

                Try
                    db.RunProc("proc_VaccineCentreSPMapping_getAll", dt)

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_VaccineCentreSPMapping, dt)
                Catch ex As Exception
                    Throw
                End Try
            End If

            Return dt

        End Function

#End Region

#Region "Get Vaccine Lot"
        Public Function GetCOVID19VaccineLotMapping(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Nullable(Of Integer), ByVal dtmServiceDate As DateTime) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@Service_Dtm", SqlDbType.DateTime, 2, dtmServiceDate)}

            db.RunProc("proc_COVID19VaccineLotMapping_get_ForPrivate", parms, dt)

            Return dt

        End Function

        'CRE20-023 immue record [Start][Nichole]
        Public Function GetCOVID19VaccineLotMappingForCentre(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Nullable(Of Integer), ByVal dtmServiceDate As DateTime) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@Service_Dtm", SqlDbType.DateTime, 8, IIf(dtmServiceDate = DateTime.MinValue, DBNull.Value, dtmServiceDate))}

            db.RunProc("proc_COVID19VaccineLotMapping_get_ForCentre", parms, dt)

            Return dt

        End Function

        Public Function GetCOVID19VaccineLotMappingForPrivate(Optional ByVal strSPID As String = "", Optional ByVal intPracticeDisplaySeq As Nullable(Of Integer) = Nothing, Optional ByVal dtmServiceDate As DateTime = Nothing) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@Service_Dtm", SqlDbType.DateTime, 8, IIf(dtmServiceDate = DateTime.MinValue, DBNull.Value, dtmServiceDate))}

            db.RunProc("proc_COVID19VaccineLotMapping_get_ForPrivate", parms, dt)

            Return dt

        End Function

        Public Function GetCOVID19VaccineLotMappingForRCH(Optional ByVal strSPID As String = "", Optional ByVal intPracticeDisplaySeq As Nullable(Of Integer) = Nothing, Optional ByVal dtmServiceDate As DateTime = Nothing) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq), _
                db.MakeInParam("@Service_Dtm", SqlDbType.DateTime, 8, IIf(dtmServiceDate = DateTime.MinValue, DBNull.Value, dtmServiceDate))}

            db.RunProc("proc_COVID19VaccineLotMapping_get_ForRCH", parms, dt)

            Return dt

        End Function

        Public Function GetALLCOVID19VaccineLotMappingForCentre(Optional ByVal strSPID As String = "", Optional ByVal intPracticeDisplaySeq As Nullable(Of Integer) = Nothing) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                    db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq)}

            db.RunProc("proc_COVID19VaccineLotMapping_getAll_ForCentre", parms, dt)

            Return dt

        End Function

        Public Function GetALLCOVID19VaccineLotMappingForPrivate(Optional ByVal strSPID As String = "", Optional ByVal intPracticeDisplaySeq As Nullable(Of Integer) = Nothing) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim objPracticeDisplaySeq As Object = DBNull.Value
            If intPracticeDisplaySeq.HasValue Then
                objPracticeDisplaySeq = intPracticeDisplaySeq.Value
            End If

            Dim parms() As SqlParameter = { _
                    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                    db.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, objPracticeDisplaySeq)}

            db.RunProc("proc_COVID19VaccineLotMapping_getAll_ForPrivate", parms, dt)

            Return dt

        End Function
        'CRE20-023 Immu record [End][Nichole]

        Public Function GetCOVID19VaccineLotMappingByVaccineLotNo(ByVal strVaccineLotNo As String) As DataTable
            'Dim dt As DataTable = Me.GetCOVID19VaccineLotMapping()
            Dim dt As DataTable = Me.GetCOVID19VaccineBrandLotDetail()
            Dim dr() As DataRow

            dr = dt.Select(String.Format("Vaccine_Lot_No = '{0}'", strVaccineLotNo))

            If dr.Length = 1 Then
                Return dr.CopyToDataTable
            Else
                Throw New Exception(String.Format("Invalid Vaccine Lot No ({0}), row(s) more than 1 or no record", strVaccineLotNo))
            End If

        End Function

        Public Function GetVaccineBrandPrintoutName(ByVal strBrand As String) As String
            Dim strResult As String = String.Empty
            Dim dt As DataTable = (New COVID19.COVID19BLL).GetCOVID19VaccineBrand()
            Dim dr() As DataRow = dt.Select(String.Format("Brand_ID = '{0}'", strBrand.Trim))

            If dr.Length > 0 Then
                strResult = dr(0)("Brand_Printout_Name")
            End If

            Return strResult

        End Function

        Public Function GetVaccineBrandPrintoutNameChi(ByVal strBrand As String) As String
            Dim strResult As String = String.Empty
            Dim dt As DataTable = (New COVID19.COVID19BLL).GetCOVID19VaccineBrand()
            Dim dr() As DataRow = dt.Select(String.Format("Brand_ID = '{0}'", strBrand.Trim))

            If dr.Length > 0 Then
                strResult = dr(0)("Brand_Printout_Name_Chi")
            End If

            Return strResult

        End Function

        Public Function GetVaccineTradeName(ByVal strBrand As String) As String
            Dim strResult As String = String.Empty
            Dim dt As DataTable = (New COVID19.COVID19BLL).GetCOVID19VaccineBrand()
            Dim dr() As DataRow = dt.Select(String.Format("Brand_ID = '{0}'", strBrand.Trim))

            If dr.Length > 0 Then
                strResult = dr(0)("Brand_Trade_Name")
            End If

            Return strResult

        End Function

        Public Function GetVaccineTradeNameChi(ByVal strBrand As String) As String
            Dim strResult As String = String.Empty
            Dim dt As DataTable = (New COVID19.COVID19BLL).GetCOVID19VaccineBrand()
            Dim dr() As DataRow = dt.Select(String.Format("Brand_ID = '{0}'", strBrand.Trim))

            If dr.Length > 0 Then
                strResult = dr(0)("Brand_Trade_Name_Chi")
            End If

            Return strResult

        End Function
#End Region

#Region "Get Vaccination Centre"
        Public Function GetCOVID19VaccineCentreHCVUMapping(ByVal strVUID As String) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strVUID)}

            db.RunProc("proc_COVID19VaccineLotCentreHCVUMapping", parms, dt)

            Return dt

        End Function

        Public Function GetCOVID19VaccineCentreBySPID(ByVal strSPID As String) As DataTable
            Dim dt As DataTable = Me.GetVaccineCentreSPMapping()
            Dim dr() As DataRow

            dr = dt.Select(String.Format("SP_ID = '{0}'", strSPID))

            If dr.Length > 0 Then
                Return dr.CopyToDataTable
            Else
                Return Nothing
            End If

        End Function

        Public Function GetCOVID19VaccineCentreBySPIDPracticeDisplaySeq(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer) As DataTable
            Dim dt As DataTable = Me.GetVaccineCentreSPMapping()
            Dim dr() As DataRow

            dr = dt.Select(String.Format("SP_ID = '{0}' AND Practice_Display_Seq = {1}", strSPID, intPracticeDisplaySeq))

            If dr.Length = 1 Then
                Return dr.CopyToDataTable
            Else
                Throw New Exception(String.Format("Invalid Vaccine Centre SP Mapping ('{0}','{1}'), row(s) more than 1 or no record", strSPID, intPracticeDisplaySeq))
            End If

        End Function
#End Region

#Region "Get Physcial Practice & SP mapping"
        Public Function GetPracticePhysicalMappingBySPIDPracticeDisplaySeq(ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer) As DataTable
            Dim dt As New DataTable
            Dim db As New Database

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID), _
                db.MakeInParam("@practice_display_seq", SqlDbType.SmallInt, 2, intPracticeDisplaySeq) _
            }

            db.RunProc("proc_PracticePhysicalMapping_get_bySPID_DisplaySeq", parms, dt)

            Return dt

        End Function
#End Region

#Region "Get COVID19 Transaction"
        Public Function GetLatestCovid19TransactionByDocId(ByVal udtSearchCriteria As Common.SearchCriteria.SearchCriteria) As DataTable
            Dim dt As New DataTable
            Dim db As New Common.DataAccess.Database
            Try
                ' create data object and params
                Dim prams() As SqlParameter = {db.MakeInParam("@doc_code", SqlDbType.Char, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentType), DBNull.Value, udtSearchCriteria.DocumentType)), _
                                                        db.MakeInParam("@identity_no1", SqlDbType.VarChar, 20, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo1), DBNull.Value, udtSearchCriteria.DocumentNo1)), _
                                                        db.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, IIf(udcValidator.IsEmpty(udtSearchCriteria.DocumentNo2), DBNull.Value, udtSearchCriteria.DocumentNo2)) _
                                                       }

                ' run the stored procedure
                db.RunProc("proc_LatestCOVID19Transaction_get_byDocId", prams, dt)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return dt
        End Function
#End Region

#Region "Supported Function"
        Public Function GenerateVaccineLotNoMappingJavaScript(ByVal strVaccineLotNoJson As String) As String
            Dim strVaccineLotNoMappingJavaScript As String = String.Empty

            'strVaccineLotNoMappingJavaScript += "var Vaccine = {'1': ['SNV202100001', 'SNV202100002'],'2': ['BNT202100001']};"
            strVaccineLotNoMappingJavaScript += "var Vaccine = " & strVaccineLotNoJson & ";"
            Dim guidText As String = Guid.NewGuid().ToString()
            strVaccineLotNoMappingJavaScript += "RemoveUsedBlockScript('" & guidText & "');"

            Return strVaccineLotNoMappingJavaScript

        End Function

        Public Function GenerateVaccineLotNoJavaScript() As String
            Dim strJsBindingddlCVaccineLotNoCovid19 As String = String.Empty

            Dim strPleaseSelect As String = HttpContext.GetGlobalResourceObject("Text", "PleaseSelect")

            strJsBindingddlCVaccineLotNoCovid19 += "var $lotNo = $(""[id$='ddlCVaccineLotNoCovid19']"");"
            'When the ddlCCategoryCovid19 is changed, save the selected value.
            strJsBindingddlCVaccineLotNoCovid19 += "$(""[id$='ddlCCategoryCovid19']"").change(function () {"
            strJsBindingddlCVaccineLotNoCovid19 += "var Category = $(this).val();"
            strJsBindingddlCVaccineLotNoCovid19 += "$(""[id$='txtCCategory']"").val(Category);"
            strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked ddl:  ""  + Category);"
            strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked txtCCategory:  ""  + $(""[id$='txtCCategory']"").val());"
            strJsBindingddlCVaccineLotNoCovid19 += "});"
            'When the ddlCVaccineBrandCovid19 is changed, it focus to rerender the ddlCVaccineLotNoCovid19.
            strJsBindingddlCVaccineLotNoCovid19 += "$(""[id$='ddlCVaccineBrandCovid19']"").change(function () {"
            strJsBindingddlCVaccineLotNoCovid19 += "var Vacc = $(this).val(), lcns = Vaccine[Vacc] || [];"
            strJsBindingddlCVaccineLotNoCovid19 += "$(""[id$='txtCVaccineBrand']"").val(Vacc);"
            'strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked ddl:  ""  + Vacc);"
            'strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked txtCVaccineBrand:  ""  + $(""[id$='txtCVaccineBrand']"").val());"
            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            strJsBindingddlCVaccineLotNoCovid19 += "var html = (lcns.length > 1 ?'<option value>" & strPleaseSelect & "</option>': '');"
            strJsBindingddlCVaccineLotNoCovid19 += "html += $.map(lcns, function(lcn){"
            strJsBindingddlCVaccineLotNoCovid19 += "return ('<option value=""' + lcn + '"">' + lcn + '</option>')"
            strJsBindingddlCVaccineLotNoCovid19 += "}).join('');"
            strJsBindingddlCVaccineLotNoCovid19 += "$lotNo.html(html).change();"
            strJsBindingddlCVaccineLotNoCovid19 += "(lcns.length > 0) ? $lotNo.prop('disabled', false) : $lotNo.prop('disabled', true);"
            strJsBindingddlCVaccineLotNoCovid19 += "});"
            'Save the ddlCVaccineLotNoCovid19 selected value to hidden text box for postback
            strJsBindingddlCVaccineLotNoCovid19 += "$lotNo.change(function () {"
            strJsBindingddlCVaccineLotNoCovid19 += "var SelectedLotNo = $(this).val();"
            strJsBindingddlCVaccineLotNoCovid19 += "$(""[id$='txtCVaccineLotNo']"").val(SelectedLotNo);"
            'strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked ddl:  ""  + SelectedLotNo);"
            'strJsBindingddlCVaccineLotNoCovid19 += "console.log(""checked txtCVaccineLotNo:  ""  + $(""[id$='txtCVaccineLotNo']"").val());"
            strJsBindingddlCVaccineLotNoCovid19 += "});"
            Dim guidText As String = Guid.NewGuid().ToString()
            strJsBindingddlCVaccineLotNoCovid19 += "RemoveUsedBlockScript('" & guidText & "');"

            Return strJsBindingddlCVaccineLotNoCovid19

        End Function

        Public Function GenerateCategoryMappingJavaScript(ByVal strCategoryDataJson As String, ByVal strCategoryTextEngJson As String, ByVal strCategoryTextChiJson As String) As String
            Dim strSubCategoryJavaScript As String = String.Empty

            'strSubCategoryJavaScript += "var SubCategory = {'PG1': ['GL1', 'GL2'],'PG2': ['GL3']};"
            strSubCategoryJavaScript += "var SubCategory = " & strCategoryDataJson & ";"
            strSubCategoryJavaScript += "var SubCategoryTextEng = " & strCategoryTextEngJson & ";"
            strSubCategoryJavaScript += "var SubCategoryTextChi = " & strCategoryTextChiJson & ";"
            Dim guidText As String = Guid.NewGuid().ToString()
            strSubCategoryJavaScript += "RemoveUsedBlockScript('" & guidText & "');"

            Return strSubCategoryJavaScript

        End Function

        Public Function GenerateCategoryJavaScript(ByVal strLang As String) As String
            Dim strJSBindingDDLCategory As String = String.Empty

            Dim strPleaseSelect As String = HttpContext.GetGlobalResourceObject("Text", "PleaseSelect")

            strJSBindingDDLCategory += "var subCategoryCOVID19Language = '" & strLang & "';"
            strJSBindingDDLCategory += "var $SubCategory = $(""[id$='ddlCSubCategoryCovid19']"");"
            'When the ddlCVaccineBrandCovid19 is changed, it focus to rerender the ddlCVaccineLotNoCovid19.
            strJSBindingDDLCategory += "$(""[id$='ddlCMainCategoryCovid19']"").change(function () {"
            strJSBindingDDLCategory += "var MainCategory = $(this).val(), lmcns = SubCategory[MainCategory] || [];"
            strJSBindingDDLCategory += "$(""[id$='txtCMainCategory']"").val(MainCategory);"
            'strJSBindingDDLCategory += "console.log(""checked ddl:  ""  + MainCategory);"
            'strJSBindingDDLCategory += "console.log(""checked txtCMainCategory:  ""  + $(""[id$='txtCMainCategory']"").val());"
            'If no. of items is more than 1, then add "Please Select" at the top of dropdownlist
            strJSBindingDDLCategory += "var html = (lmcns.length > 1 ?'<option value>" & strPleaseSelect & "</option>': '');"
            strJSBindingDDLCategory += "html += $.map(lmcns, function(lmcn){"
            'strJSBindingDDLCategory += "var opt;if (subCategoryCOVID19Language =='zh-tw'||subCategoryCOVID19Language=='zh'){"
            strJSBindingDDLCategory += "var opt;if (subCategoryCOVID19Language =='zh-tw'){"
            strJSBindingDDLCategory += "opt='<option value=""' + lmcn + '"">' + SubCategoryTextChi[lmcn] + '</option>';}else{"
            strJSBindingDDLCategory += "opt='<option value=""' + lmcn + '"">' + SubCategoryTextEng[lmcn] + '</option>';}"
            strJSBindingDDLCategory += "return opt;"
            strJSBindingDDLCategory += "}).join('');"
            strJSBindingDDLCategory += "$SubCategory.html(html).change();"
            strJSBindingDDLCategory += "(lmcns.length > 0) ? $SubCategory.prop('disabled', false) : $SubCategory.prop('disabled', true);"
            strJSBindingDDLCategory += "});"
            'Save the ddlCSubCategoryCovid19 selected value to hidden text box for postback
            strJSBindingDDLCategory += "$SubCategory.change(function () {"
            strJSBindingDDLCategory += "var SelectedSubCategory = $(this).val();"
            strJSBindingDDLCategory += "$(""[id$='txtCSubCategory']"").val(SelectedSubCategory);"
            'strJSBindingDDLCategory += "console.log(""checked ddl:  ""  + SelectedSubCategory);"
            'strJSBindingDDLCategory += "console.log(""checked txtCVaccineLotNo:  ""  + $(""[id$='txtCSubCategory']"").val());"
            strJSBindingDDLCategory += "});"
            Dim guidText As String = Guid.NewGuid().ToString()
            strJSBindingDDLCategory += "RemoveUsedBlockScript('" & guidText & "');"

            Return strJSBindingDDLCategory

        End Function

        Public Function FilterActiveVaccineLotNoByServiceDate(ByVal dtVaccineLotNo As DataTable, ByVal dtmServiceDate As DateTime) As DataRow()
            Dim drVaccineLotNo() As DataRow = Nothing

            'CRE20-023 Fix the lot mapping filter for HCVU [Start][Nichole]
            drVaccineLotNo = dtVaccineLotNo.Select(String.Format("'{0}' >= Service_Period_From AND '{1}' <= Service_Period_To and record_status= '{2}'", dtmServiceDate, dtmServiceDate, VaccineLotMappingRecordStatus.Active))
            'CRE20-023 Fix the lot mapping filter for HCVU [End][Nichole]

            If drVaccineLotNo.Length > 0 Then
                Dim dt As DataTable = drVaccineLotNo.CopyToDataTable
                Dim dtSort As DataTable = Nothing

                dt.DefaultView.Sort = "Vaccine_Lot_No"
                dtSort = dt.DefaultView.ToTable

                drVaccineLotNo = dtSort.Select()
            End If

            Return drVaccineLotNo

        End Function

        Public Function GenerateVaccineLotNoJson(ByVal drVaccineLotNo As DataRow()) As String
            Dim strVaccineLotNoJson As String = String.Empty

            If drVaccineLotNo IsNot Nothing AndAlso drVaccineLotNo.Length > 0 Then
                'Convert DataRow to Dictionary(select Vaccine_Lot_No group by Brand_ID)
                Dim dicVaccineLotNo As Dictionary(Of String, String()) = drVaccineLotNo _
                    .GroupBy(Function(row) row.Field(Of String)("Brand_ID")) _
                    .ToDictionary(
                        Function(gp) gp.Key,
                        Function(gp) gp.Select(Function(gpRow) gpRow.Field(Of String)("Vaccine_Lot_No")).ToArray()
                    )

                'Convert Dictionary to josn
                Dim serializer As New JavaScriptSerializer()
                strVaccineLotNoJson = serializer.Serialize(dicVaccineLotNo)
            End If

            Return strVaccineLotNoJson

        End Function

        Public Sub GenerateCategoryJson(ByRef strCategoryData As String, ByRef strCategoryTextEng As String, ByRef strCategoryTextChi As String)
            Dim dtSubCategory As DataTable = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory")
            Dim drSubcategory() As DataRow = dtSubCategory.Select()

            If drSubcategory IsNot Nothing AndAlso drSubcategory.Length > 0 Then
                'Convert DataRow to Dictionary(select SubCategory group by Brand_ID)
                Dim dicSubCategory As Dictionary(Of String, String()) = drSubcategory _
                    .GroupBy(Function(row) row.Field(Of String)("Column_Name")) _
                    .ToDictionary(
                        Function(gp) gp.Key,
                        Function(gp) gp.Select(Function(gpRow) gpRow.Field(Of String)("Status_Value")).ToArray()
                    )

                Dim dicSubCategoryTextEng As Dictionary(Of String, String()) = drSubcategory _
                    .GroupBy(Function(row) row.Field(Of String)("Status_Value")) _
                    .ToDictionary(
                        Function(gp) gp.Key,
                        Function(gp) gp.Select(Function(gpRow) gpRow.Field(Of String)("Status_Description")).ToArray()
                    )

                Dim dicSubCategoryTextChi As Dictionary(Of String, String()) = drSubcategory _
                    .GroupBy(Function(row) row.Field(Of String)("Status_Value")) _
                    .ToDictionary(
                        Function(gp) gp.Key,
                        Function(gp) gp.Select(Function(gpRow) gpRow.Field(Of String)("Status_Description_Chi")).ToArray()
                    )

                'Convert Dictionary to josn
                Dim serializer As New JavaScriptSerializer()

                strCategoryData = serializer.Serialize(dicSubCategory)
                strCategoryTextEng = serializer.Serialize(dicSubCategoryTextEng)
                strCategoryTextChi = serializer.Serialize(dicSubCategoryTextChi)
            End If

        End Sub

#End Region

#Region "QR Code"
        Public Function GetQRCodeVersion() As String
            Dim udtGeneralFunction As New GeneralFunction

            'Get QRCode Version
            Return udtGeneralFunction.GetSystemParameterParmValue1("COVID19_Vaccination_Record_QRCode_QRCodeVersion")

        End Function

        Public Function GetKeyVersion() As String
            Dim udtGeneralFunction As New GeneralFunction

            'Get QRCode Version
            Return udtGeneralFunction.GetSystemParameterParmValue1("COVID19_Vaccination_Record_QRCode_KeyVersion")

        End Function

        Public Function GetPublicKey() As String
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParm1 As String = String.Empty
            Dim strParm2 As String = String.Empty

            'Get QRCode Version
            udtGeneralFunction.getSystemParameter("COVID19_Vaccination_Record_QRCode_KeyVersion", strParm1, strParm2)

            Return strParm2

        End Function

        Public Function GenerateDigitalSignature(ByVal strRawData As String) As QRCodeModel
            Dim udtQRCode As QRCodeModel = Nothing
            Dim udtGeneralFunction As New GeneralFunction

            Dim strPrivateKey As String = String.Empty
            Dim strSignature As String = String.Empty

            'Get QRCode Version
            udtGeneralFunction.getSystemParameterPassword("COVID19_Vaccination_Record_QRCode_KeyVersion", strPrivateKey)

            If strPrivateKey <> String.Empty Then
                udtQRCode = New QRCodeModel

                strSignature = Signature.Sign(strRawData, Signature.GetECPrivateKeyParameters(strPrivateKey, False))

                udtQRCode.Signature = strSignature

            End If

            Return udtQRCode

        End Function

        Public Function VerifyDigitalSignature(ByVal strRawData As String, ByVal strSignedData As String) As Boolean
            Dim udtQRCode As QRCodeModel = Nothing
            Dim udtGeneralFunction As New GeneralFunction

            Dim strPrivateKey As String = String.Empty
            Dim blnValid As Boolean = False

            'Get QRCode Version
            udtGeneralFunction.getSystemParameterPassword("COVID19_Vaccination_Record_QRCode_KeyVersion", strPrivateKey)

            If strPrivateKey <> String.Empty Then
                blnValid = Signature.VerifySignature(strRawData, Signature.GetECPublicKeyParameters(strPrivateKey, False), strSignedData)
            End If

            Return blnValid

        End Function
#End Region

    End Class
End Namespace

