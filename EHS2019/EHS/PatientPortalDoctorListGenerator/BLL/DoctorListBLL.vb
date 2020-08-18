Imports Common.DataAccess
Imports Common.ComFunction.AccountSecurity
Imports Common.eHRIntegration.Model.DoctorList
Imports PatientPortalDoctorListGenerator.Logging
Imports System.Data.SqlClient

Public Class DoctorListBLL

    Private udtDB As New Database

    Public Function GetAllData() As DataSet
        Dim ds As New DataSet

        udtDB.RunProc("proc_PatientPortalDoctorListData_get_All", ds)

        Return ds

    End Function

    Public Function ProcessDataToXmlModel(ByVal ds As DataSet) As root
        Dim udtXML As root = Nothing
        Dim udtCodeTable As CodeTableModel = Nothing
        Dim blnValid As Boolean = True

        If ds.Tables.Count > 0 AndAlso ds.Tables.Count <> 8 Then
            Throw New Exception("Input dataset is invalid.")
        End If

        Dim dt As DataTable

        '---------------------------------------
        ' 1. System Name
        '---------------------------------------
        Dim strSystem As String = String.Empty

        dt = ds.Tables(0)

        strSystem = dt.Rows(0)("System").Trim

        '---------------------------------------
        ' 2. SDIR Update DateTime
        '---------------------------------------
        Dim strGenerationDateTime As String = String.Empty

        dt = ds.Tables(1)

        strGenerationDateTime = dt.Rows(0)("Generation_DateTime").Trim

        '---------------------------------------
        ' 3. Points to Note
        '---------------------------------------
        Dim udtPointsToNoteList As New PointsToNoteModelCollection
        Dim udtPointsToNote As item = Nothing

        dt = ds.Tables(2)

        For Each dr As DataRow In dt.Rows
            udtPointsToNote = New item
            udtPointsToNote.item_level = dr("Item_Level").Trim
            udtPointsToNote.item_scheme_code = IIf(IsDBNull(dr("Item_Scheme_Code")), String.Empty, dr("Item_Scheme_Code")).Trim
            udtPointsToNote.item_desc_en = dr("Item_Desc_EN").Trim
            udtPointsToNote.item_desc_tc = dr("Item_Desc_TC").Trim
            udtPointsToNoteList.Add(udtPointsToNote)
        Next

        '---------------------------------------
        ' 4. Code Table - District
        '---------------------------------------
        Dim udtCodeDistrictList As New CodeDistrictModelCollection
        Dim udtCodeDistrict As district_item = Nothing

        dt = ds.Tables(3)

        For Each dr As DataRow In dt.Rows
            udtCodeDistrict = New district_item
            udtCodeDistrict.district_code = dr("District_Code").Trim
            udtCodeDistrict.district_name_en = dr("District_Desc_EN").Trim
            udtCodeDistrict.district_name_tc = dr("District_Desc_TC").Trim
            udtCodeDistrictList.Add(udtCodeDistrict)
        Next

        '---------------------------------------
        ' 5. Code Table - Profession
        '---------------------------------------
        Dim udtCodeProfessionList As New CodeProfessionModelCollection
        Dim udtCodeProfession As profession_item = Nothing

        dt = ds.Tables(4)

        For Each dr As DataRow In dt.Rows
            udtCodeProfession = New profession_item
            udtCodeProfession.prof_code = dr("Prof_Code").Trim
            udtCodeProfession.prof_name_en = dr("Prof_Name_EN").Trim
            udtCodeProfession.prof_name_tc = dr("Prof_Name_TC").Trim
            udtCodeProfessionList.Add(udtCodeProfession)
        Next

        '---------------------------------------
        ' 6. Code Table - Scheme
        '---------------------------------------
        Dim udtCodeSchemeList As New CodeSchemeModelCollection
        Dim udtCodeScheme As scheme_item = Nothing

        dt = ds.Tables(5)

        For Each dr As DataRow In dt.Rows
            udtCodeScheme = New scheme_item
            udtCodeScheme.scheme_code = dr("Scheme_Code").Trim
            udtCodeScheme.scheme_name_en = dr("Scheme_Name_EN").Trim
            udtCodeScheme.scheme_name_tc = dr("Scheme_Name_TC").Trim
            udtCodeScheme.scheme_email = dr("Scheme_Email").Trim
            udtCodeScheme.scheme_tel_no = dr("Scheme_Tel_No").Trim
            udtCodeScheme.scheme_website_en = dr("Scheme_Website_EN").Trim
            udtCodeScheme.scheme_website_tc = dr("Scheme_Website_TC").Trim
            udtCodeScheme.scheme_website_sc = dr("Scheme_Website_SC").Trim
            udtCodeSchemeList.Add(udtCodeScheme)
        Next

        '---------------------------------------
        ' 7. Code Table - Category
        '---------------------------------------
        'Dim udtCodeCategoryList As New CodeCategoryModelCollection
        'Dim udtCodeCategory As category_item = Nothing

        'dt = ds.Tables(6)

        'For Each dr As DataRow In dt.Rows
        '    udtCodeCategory = New category_item
        '    udtCodeCategory.Category_Code = dr("Category_Code").Trim
        '    udtCodeCategory.Category_Name_EN = dr("Category_Name_EN").Trim
        '    udtCodeCategory.Category_Name_TC = dr("Category_Name_TC").Trim
        '    udtCodeCategoryList.Add(udtCodeCategory)
        'Next

        '---------------------------------------
        ' 8. Code Table - Vaccine
        '---------------------------------------
        'Dim udtCodeVaccineList As New CodeVaccineModelCollection
        'Dim udtCodeVaccine As vaccine_item = Nothing

        'dt = ds.Tables(7)

        'For Each dr As DataRow In dt.Rows
        '    udtCodeVaccine = New vaccine_item
        '    udtCodeVaccine.Vaccine_Code = dr("Vaccine_Code").Trim
        '    udtCodeVaccine.Vaccine_Name_EN = dr("Vaccine_Name_EN").Trim
        '    udtCodeVaccine.Vaccine_Name_TC = dr("Vaccine_Name_TC").Trim
        '    udtCodeVaccine.Vaccine_Short_Form = dr("Vaccine_Short_Form").Trim
        '    udtCodeVaccineList.Add(udtCodeVaccine)
        'Next

        '---------------------------------------
        ' 9. SP + Practice
        '---------------------------------------
        Dim dtSP As DataTable = Nothing
        Dim dtSPID As DataTable = Nothing
        Dim dtPractice As DataTable = Nothing
        Dim dtPracticeID As DataTable = Nothing
        Dim dtPracticeScheme As DataTable = Nothing
        Dim dtPracticeSchemeCode As DataTable = Nothing
        Dim dtCategory As DataTable = Nothing
        Dim dtCategoryCode As DataTable = Nothing
        Dim dtVaccine As DataTable = Nothing
        Dim dtVaccineCode As DataTable = Nothing
        Dim dtServiceFee As DataTable = Nothing

        Dim drPracticeScheme As DataRow() = Nothing
        Dim drCategories As DataRow() = Nothing

        Dim udtSPList As SPModelCollection = Nothing
        Dim udtPracticeList As PracticeModelCollection = Nothing
        Dim udtPracticeSchemeList As PracticeSchemeModelCollection = Nothing
        Dim udtCategoryList As CategoryModelCollection = Nothing
        Dim udtVaccineList As VaccineModelCollection = Nothing

        Dim udtSP As sp = Nothing
        Dim udtPractice As practice = Nothing
        Dim udtPracticeScheme As scheme = Nothing
        Dim udtCategory As category = Nothing
        Dim udtVaccine As vaccine = Nothing

        'SP Level
        dtSPID = ds.Tables(6).DefaultView.ToTable(True, "SP_ID")
        udtSPList = New SPModelCollection

        For Each drSPID As DataRow In dtSPID.Rows
            ConsoleLog(String.Format("SPID({0}) start.", drSPID("SP_ID")))

            dtPracticeID = Nothing

            dtSP = ds.Tables(6).Select(String.Format("SP_ID='{0}'", drSPID("SP_ID"))).CopyToDataTable

            udtSP = New sp
            udtSP.sp_id = Hash(dtSP(0)("SP_ID")).HashedValue.Trim
            udtSP.sp_name_en = IIf(IsDBNull(dtSP(0)("SP_Name_EN")), String.Empty, dtSP(0)("SP_Name_EN")).Trim()
            udtSP.sp_name_tc = IIf(IsDBNull(dtSP(0)("SP_Name_TC")), String.Empty, dtSP(0)("SP_Name_TC")).Trim()

            'Practice Level
            dtPracticeID = dtSP.DefaultView.ToTable(True, "Practice_ID")
            udtPracticeList = New PracticeModelCollection

            For Each drPracticeID As DataRow In dtPracticeID.Rows
                'ConsoleLog(String.Format("SPID({0}) - Pracatice({1}) start.", drSPID("SP_ID"), drPracticeID("Practice_ID")))

                dtPracticeSchemeCode = Nothing

                dtPractice = dtSP.Select(String.Format("Practice_ID='{0}'", drPracticeID("Practice_ID"))).CopyToDataTable

                udtPractice = New practice
                udtPractice.practice_id = dtPractice(0)("Practice_ID").ToString.Trim
                udtPractice.practice_name_en = dtPractice(0)("Practice_Name_EN").Trim
                udtPractice.practice_name_tc = dtPractice(0)("Practice_Name_TC").Trim
                udtPractice.practice_addr_en = dtPractice(0)("Practice_Addr_EN").Trim
                udtPractice.practice_addr_tc = dtPractice(0)("Practice_Addr_TC").Trim
                udtPractice.practice_tel_no = dtPractice(0)("Practice_Tel_No").trim
                udtPractice.practice_district_code = dtPractice(0)("Practice_District_Code").Trim
                udtPractice.practice_prof_code = dtPractice(0)("Practice_Prof_Code").Trim
                udtPractice.practice_prof_reg_no = dtPractice(0)("Practice_Prof_Reg_No").Trim

                'Practice Scheme Level
                drPracticeScheme = ds.Tables(7).Select(String.Format("SP_ID='{0}' AND Practice_ID='{1}' AND Scheme_Code IS NOT NULL", _
                                                                         dtSP(0)("SP_ID"), dtPractice(0)("Practice_ID")))

                If drPracticeScheme.Length > 0 Then
                    dtPracticeScheme = drPracticeScheme.CopyToDataTable
                    dtPracticeSchemeCode = drPracticeScheme.CopyToDataTable.DefaultView.ToTable(True, "Scheme_Code")
                End If

                If Not dtPracticeSchemeCode Is Nothing AndAlso dtPracticeSchemeCode.Rows.Count > 0 Then

                    udtPracticeSchemeList = New PracticeSchemeModelCollection

                    For Each drPracticeSchemeCode As DataRow In dtPracticeSchemeCode.Rows
                        'ConsoleLog(String.Format("SPID({0}) - Pracatice({1}) - Scheme {2} start.", drSPID("SP_ID"), drPracticeID("Practice_ID"), drPracticeSchemeCode("Scheme_Code")))

                        dtCategoryCode = Nothing

                        udtPracticeScheme = New scheme
                        udtPracticeScheme.scheme_code = drPracticeSchemeCode("Scheme_Code")

                        'Practice Scheme - Category Level
                        dtCategory = dtPracticeScheme.Select(String.Format("Scheme_Code='{0}'", drPracticeSchemeCode("Scheme_Code"))).CopyToDataTable()

                        drCategories = dtCategory.Select("Category_Code IS NOT NULL")

                        If drCategories.Length > 0 Then
                            dtCategoryCode = drCategories.CopyToDataTable.DefaultView.ToTable(True, "Category_Code")
                        End If

                        If Not dtCategoryCode Is Nothing AndAlso dtCategoryCode.Rows.Count > 0 Then
                            udtCategoryList = New CategoryModelCollection()

                            For Each drCategoryCode As DataRow In dtCategoryCode.Rows
                                dtVaccineCode = Nothing

                                dtVaccine = dtCategory.Select(String.Format("Category_Code='{0}'", drCategoryCode("Category_Code"))).CopyToDataTable()

                                udtCategory = New category
                                udtCategory.category_code = dtVaccine(0)("Category_Code")

                                'Practice Scheme - Category - Vaccine Level
                                dtVaccineCode = dtVaccine.DefaultView.ToTable(True, "Vaccine_Code")
                                udtVaccineList = New VaccineModelCollection

                                For Each drVaccineCode As DataRow In dtVaccineCode.Rows

                                    dtServiceFee = dtVaccine.Select(String.Format("Vaccine_Code='{0}'", drVaccineCode("Vaccine_Code"))).CopyToDataTable()

                                    udtVaccine = New vaccine
                                    udtVaccine.vaccine_code = dtServiceFee(0)("Vaccine_Code").ToString.Trim
                                    udtVaccine.vaccine_service_fee_provided = dtServiceFee(0)("Vaccine_Service_Fee_Provided").ToString.Trim
                                    udtVaccine.vaccine_service_fee = IIf(IsNumeric(dtServiceFee(0)("Vaccine_Service_Fee")), CInt(dtServiceFee(0)("Vaccine_Service_Fee")).ToString, dtServiceFee(0)("Vaccine_Service_Fee"))
                                    udtVaccine.vaccine_service_fee_remark_en = String.Empty
                                    udtVaccine.vaccine_service_fee_remark_tc = String.Empty

                                    udtVaccineList.Add(udtVaccine)
                                Next

                                If udtVaccineList.Count = 0 Then
                                    Continue For
                                End If

                                udtCategory.vaccine_list = udtVaccineList

                                udtCategoryList.Add(udtCategory)

                            Next

                            If udtCategoryList.Count = 0 Then
                                Continue For
                            End If

                            udtPracticeScheme.category_list = udtCategoryList

                        End If

                        udtPracticeSchemeList.Add(udtPracticeScheme)

                    Next

                    If udtPracticeSchemeList.Count = 0 Then
                        Continue For
                    End If

                    udtPractice.scheme_list = udtPracticeSchemeList

                    udtPracticeList.Add(udtPractice)

                End If

            Next

            If udtPracticeList.Count = 0 Then
                Continue For
            End If

            udtSP.practice_list = udtPracticeList

            udtSPList.Add(udtSP)

            ConsoleLog(String.Format("SPID({0}) is successful.", drSPID("SP_ID")))
        Next

        udtCodeTable = New CodeTableModel()
        'udtCodeTable.category_item_list = udtCodeCategoryList
        udtCodeTable.district_item_list = udtCodeDistrictList
        udtCodeTable.profession_item_list = udtCodeProfessionList
        udtCodeTable.scheme_item_list = udtCodeSchemeList
        'udtCodeTable.vaccine_item_list = udtCodeVaccineList

        udtXML = New root
        udtXML.system = strSystem
        udtXML.generation_datetime = strGenerationDateTime
        udtXML.points_to_note = udtPointsToNoteList
        udtXML.code_table = udtCodeTable
        udtXML.sp_list = udtSPList

        Return udtXML

    End Function

End Class
