Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.ComFunction.AccountSecurity

Namespace BLL

    Public Class SPProfileBLL


        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtDataEntryBLL As DataEntryUserBLL = New DataEntryUserBLL
        Private udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        Private udtProfessionBLL As ProfessionBLL = New ProfessionBLL

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        Dim udcvalidator As Common.Validation.Validator = New Common.Validation.Validator
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction
        'Private udtSchemeBLL As SchemeBLL = New SchemeBLL
        Private udtDB As Database = New Database

        Public Const SESS_SPProfileTMSP As String = "SPProfileTSMP"

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_SPProfileTMSP) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_SPProfileTMSP) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef byteTSMP As Byte())
            HttpContext.Current.Session(SESS_SPProfileTMSP) = byteTSMP
        End Sub

        Public Function LoadToSession()
            Return HttpContext.Current.Session(SESS_SPProfileTMSP)
        End Function



        ''' <summary>
        ''' Get the lists of practice type
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPracticeType() As StaticDataModelCollection
            Return udtStaticDataBLL.GetStaticDataListByColumnName("PRACTICETYPE")
        End Function

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        ''' <summary>
        ''' Get the health profession by health profession code
        ''' </summary>
        ''' <param name="strServiceCategoryCode"></param>
        ''' <returns>Health Profession Name</returns>
        ''' <remarks></remarks>
        Public Function GetHealthProfName(ByVal strServiceCategoryCode As String) As ProfessionModel
            'Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", strItemNo)
            Return udtProfessionBLL.GetProfessionListByServiceCategoryCode(strServiceCategoryCode)
        End Function

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        ''' <summary>
        ''' Get the practice type by practice type code
        ''' </summary>
        ''' <param name="strItemNo"></param>
        ''' <returns>Practice Type Name</returns>
        ''' <remarks></remarks>
        Public Function GetPracticeTypeName(ByVal strItemNo As String) As StaticDataModel
            Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", strItemNo)
        End Function

        Public Function loadSP(ByVal strSPID As String) As ServiceProviderModel
            Dim udtserviceproviderbll As ServiceProviderBLL = New ServiceProviderBLL
            Dim udtSP As ServiceProviderModel
            Dim udtDB As Database = New Database
            udtSP = udtSPBLL.GetServiceProviderBySPID(udtDB, strSPID)
            'udtSP = udtserviceproviderbll.GetServiceProviderPermanentProfileByERN(udtSP.EnrolRefNo, udtDB)
            Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
            'udtSP.MOList = udtMOBLL.GetMOListBySPID(udtSP.SPID, udtDB)
            udtMOBLL.SaveToSession(udtSP.MOList)

            'Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
            'For Each practiceM As Practice.PracticeModel In udtSP.PracticeList.Values
            '    practiceM.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtSP.SPID, practiceM.DisplaySeq, udtDB)
            'Next
            'udtserviceproviderbll.SaveToSession(udtSP)

            Return udtSP
        End Function

        Public Function chkValidIVRSPassword(ByVal strNewIVRSPWD As String) As Boolean
            Dim blnRes As Boolean = False
            blnRes = udcvalidator.ValidateIVRSPassword(strNewIVRSPWD)
            Return blnRes
        End Function

        Public Function chkValidPassword(ByVal strNewPWD As String) As Boolean
            Dim blnRes As Boolean = False
            blnRes = udcvalidator.ValidatePassword(strNewPWD)
            Return blnRes
        End Function

        Public Function chkIsIdenticalPassword(ByVal strNewPWD As String, ByVal strConfirmPWD As String) As Boolean
            Dim blnRes As Boolean = True
            If strNewPWD.Trim <> strConfirmPWD.Trim Then
                blnRes = False
            End If
            Return blnRes
        End Function

        Public Function chkIsEmpty(ByVal strChkField As String) As Boolean
            Dim blnRes As Boolean = False
            If strChkField.Trim.Equals(String.Empty) Then
                blnRes = True
            End If
            Return blnRes
        End Function

        Public Function chkValidLoginID(ByVal strLoginID As String) As Boolean
            Dim blnRes As Boolean = False
            blnRes = udcvalidator.ValidateAlias(strLoginID)
            Return blnRes
        End Function

        Public Function chkDuplicateUsername(ByVal strUsername As String) As Boolean
            Dim blnRes As Boolean = False
            Dim udtAcctActivateBLL As AccountActivationBLL = New AccountActivationBLL
            blnRes = udtAcctActivateBLL.IsAccountAliasDuplicated(strUsername)
            Return blnRes
        End Function

        Public Function loadSPLoginProfile(ByVal strSPID As String, ByVal strSchemeCode As String) As DataTable
            Dim udtDB As Database = New Database
            Dim dtRes As DataTable = New DataTable
            Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                            udtDB.MakeInParam("@Project", SqlDbType.Char, 10, strSchemeCode) _
                            }
            udtDB.RunProc("proc_HCSPUserACProfile_get_BySPID", parms, dtRes)
            Return dtRes
        End Function

        Public Function saveSPLoginProfile(ByVal strSPID As String, ByVal strOriAlias As String, ByVal strNewAlias As String, _
                    ByVal strChgWebPWD As String, ByVal strWebPWD As String, ByVal strChgIVRSPWD As String, ByVal strIVRSPWD As String, ByVal strDefaultLang As String, ByVal byteTSMP As Byte(), Optional ByVal strPrintOption As String = "") As Boolean
            Dim blnRes As Boolean = False
            Dim strEncryptPWD As String = String.Empty
            Dim strEncryptIVRSPWD As String = String.Empty
            Dim strChgAlias As String = String.Empty
            Dim udtDB As Database = New Database
            Dim udtPassword As HashModel = New HashModel
            Dim udtIVRSPassword As HashModel = New HashModel


            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
            'If strChgWebPWD = "Y" Then strEncryptPWD = Common.Encryption.Encrypt.MD5hash(strWebPWD)
            'If strChgIVRSPWD = "Y" Then strEncryptIVRSPWD = Common.Encryption.Encrypt.MD5hash(strIVRSPWD)

            If strChgWebPWD = "Y" Then
                udtPassword = Hash(strWebPWD)
                strEncryptPWD = udtPassword.HashedValue
            End If
            If strChgIVRSPWD = "Y" Then
                udtIVRSPassword = Hash(strIVRSPWD)
                strEncryptIVRSPWD = udtIVRSPassword.HashedValue
            End If

            If strOriAlias.Trim.Equals(strNewAlias.Trim) Then
                strChgAlias = "N"
            Else
                strChgAlias = "Y"
            End If

            Dim parms() As SqlParameter = { _
                         udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                         udtDB.MakeInParam("@SP_Password", SqlDbType.VarChar, 100, strEncryptPWD), _
                         udtDB.MakeInParam("@Chg_SP_Password", SqlDbType.Char, 1, strChgWebPWD), _
                         udtDB.MakeInParam("@SP_IVRS_Password", SqlDbType.VarChar, 100, strEncryptIVRSPWD), _
                         udtDB.MakeInParam("@Chg_SP_IVRS_Password", SqlDbType.Char, 1, strChgIVRSPWD), _
                         udtDB.MakeInParam("@Alias_Account", SqlDbType.VarChar, 20, strNewAlias), _
                         udtDB.MakeInParam("@Chg_Alias_Account", SqlDbType.VarChar, 1, strChgAlias), _
                         udtDB.MakeInParam("@Default_Language", SqlDbType.Char, 1, strDefaultLang), _
                         udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strSPID), _
                         udtDB.MakeInParam("@Print_Option", SqlDbType.Char, 1, strPrintOption), _
                         udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, byteTSMP), _
                         udtDB.MakeInParam("@SP_Password_Level", SqlDbType.Int, 4, udtPassword.PasswordLevel), _
                         udtDB.MakeInParam("@SP_IVRS_Password_Level", SqlDbType.Int, 4, udtIVRSPassword.PasswordLevel) _
                         }
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

            udtDB.RunProc("proc_HCSPUserAC_upd_Profile", parms)

            Return blnRes
        End Function

        'Private Function chkMatchOldPwd(ByVal strOldPWD As String, ByVal strInputPWD As String) As Boolean
        '    Dim blnRes As Boolean = False
        '    Dim strEnNewPWD As String = String.Empty

        '    strEnNewPWD = Common.Encryption.Encrypt.MD5hash(strInputPWD)

        '    If strOldPWD.Trim.Equals(strEnNewPWD.Trim) Then
        '        blnRes = True
        '    Else
        '        blnRes = False
        '    End If

        '    Return blnRes
        'End Function

        Public Function chkMatchWebPWD(ByVal dt As DataTable, ByVal strInputPWD As String) As Boolean
            Dim blnRes As Boolean = False

            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
            'blnRes = chkMatchOldPwd(dt.Rows(0).Item("SP_Password").ToString, strInputPWD)
            Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.SP, dt, strInputPWD)
            If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                blnRes = True
            End If
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

            Return blnRes
        End Function

        Public Function chkMatchIVRSPWD(ByVal dt As DataTable, ByVal strInputPWD As String) As Boolean
            Dim blnRes As Boolean = False

            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
            'blnRes = chkMatchOldPwd(dt.Rows(0).Item("SP_IVRS_Password").ToString, strInputPWD)
            Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.IVRS, dt, strInputPWD)
            If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                blnRes = True
            End If
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
            Return blnRes
        End Function

        Public Function chkIsSamePWD(ByVal strOldPWD As String, ByVal strNewPWD As String) As Boolean
            Dim blnRes As Boolean = False
            If strOldPWD.Equals(strNewPWD) Then
                blnRes = True
            Else
                blnRes = False
            End If
            Return blnRes
        End Function

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' Function Obsoleted
        ' ''' <summary>
        ' ''' Load SP Profile (For MyProfile Only)
        ' ''' Retrieve the Practice Type Also in Practice(s)
        ' ''' To Be Remove After Data migration Complete
        ' ''' </summary>
        ' ''' <param name="strSPID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function loadSP_ForMyProfileV1(ByVal strSPID As String) As ServiceProviderModel

        '    Dim udtSP As ServiceProviderModel = Nothing
        '    Dim udtserviceproviderbll As New ServiceProviderBLL()
        '    Dim udtDB As Database = New Database()

        '    udtSP = udtSPBLL.GetServiceProviderBySPID_ForMyProfileV1(udtDB, strSPID)

        '    Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
        '    udtMOBLL.SaveToSession(udtSP.MOList)

        '    Return udtSP
        'End Function
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        '==================================================================== Code for SmartID ============================================================================
        Public Function GetPilotRunSmartIDListBySPID(ByVal strSPID As String) As DataTable
            Dim udtDB As Database = New Database
            Dim dtRes As DataTable = New DataTable
            Dim parms() As SqlParameter = { _
                            udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID) _
                            }
            udtDB.RunProc("proc_PilotSmartIDServiceProvider_get_BySPID", parms, dtRes)

            Return dtRes
        End Function
        '==================================================================================================================================================================

        '==================================================================== Code for SSO ============================================================================
        Public Function CheckSSOPilotRunEligiblityBySPID(ByVal strSPID As String) As Boolean

            Dim dt As New DataTable()

            Try
                Dim parms() As SqlParameter = { _
                                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID) _
                                }
                udtDB.RunProc("proc_PilotSSOServiceProvider_get", parms, dt)

                If dt.Rows.Count = 1 Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As SqlException
                Throw ex
            Catch ex As Exception
                Throw ex
            End Try

        End Function
        '==================================================================================================================================================================



    End Class


End Namespace