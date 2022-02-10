Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component
Imports Common.Component.Address
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo

Namespace BLL

    Public Class PracticeBankAcctBLL

        Private Const strYes = "Y"
        Private Const strNo = "N"


        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()

        Public Const SESS_SelectedPractice As String = "SelectedPractice"
        Public Const SESS_SelectedPracticeName As String = "SelectedPracticeName"
        Public Const SESS_SelectedPracticeNameChi As String = "SelectedPracticeNameChi"
        Public Const SESS_SelectedPracticeAddress As String = "SelectedPracticeAddress"
        Public Const SESS_SelectedPracticeAddressChi As String = "SelectedPracticeAddressChi"
        Public Const SESS_PracticeList As String = "PracticeBankAcct_PracticeList"
        Public Const SESS_PracticeAddressList As String = "PracticeBankAcct_PracticeAddressList"

        ' To be Remove: Practice Selection Section Handling!
        Public Sub GetSelectedPractice(ByRef strPracticeValue As String, ByRef strPracticeName As String, ByRef strPracticeNameChi As String, ByRef strPracticeAddress As String, ByRef strPracticeAddressChi As String)
            strPracticeValue = CType(HttpContext.Current.Session(SESS_SelectedPractice), String)
            strPracticeName = CType(HttpContext.Current.Session(SESS_SelectedPracticeName), String)
            strPracticeNameChi = CType(HttpContext.Current.Session(SESS_SelectedPracticeNameChi), String)
            strPracticeAddress = CType(HttpContext.Current.Session(SESS_SelectedPracticeAddress), String)
            strPracticeAddressChi = CType(HttpContext.Current.Session(SESS_SelectedPracticeAddressChi), String)
        End Sub

        Public Sub SaveToSession(ByVal strPracticeValue As String, ByVal strPracticeName As String, ByRef strPracticeNameChi As String, ByVal strPracticeAddress As String, ByVal strPracticeAddressChi As String)
            HttpContext.Current.Session(SESS_SelectedPractice) = strPracticeValue
            HttpContext.Current.Session(SESS_SelectedPracticeName) = strPracticeName
            HttpContext.Current.Session(SESS_SelectedPracticeNameChi) = strPracticeNameChi
            HttpContext.Current.Session(SESS_SelectedPracticeAddress) = strPracticeAddress
            HttpContext.Current.Session(SESS_SelectedPracticeAddressChi) = strPracticeAddressChi
        End Sub

        Public Sub GetPracticeListFromSession(ByRef dtPractice As DataTable)
            dtPractice = CType(HttpContext.Current.Session(SESS_PracticeList), DataTable)
        End Sub

        Public Sub GetPracticeAddressListFromSession(ByRef dtPracticeAddress As DataTable)
            dtPracticeAddress = CType(HttpContext.Current.Session(SESS_PracticeAddressList), DataTable)
        End Sub

        Public Sub SavePracticeListToSession(ByVal dtPractice As DataTable)
            HttpContext.Current.Session(SESS_PracticeList) = dtPractice
        End Sub

        Public Sub SavePracticeAddressListToSession(ByVal dtPracticeAddress As DataTable)
            HttpContext.Current.Session(SESS_PracticeAddressList) = dtPracticeAddress
        End Sub


#Region "Static Function"

        ''' <summary>
        '''  [Static] For Handle Practice Display Chinese Information By Parameter HCSPDataMirgrationCompleteTurnOn
        ''' </summary>
        ''' <param name="dtSource"></param>
        ''' <param name="strEngFieldName"></param>
        ''' <param name="strChiFieldName"></param>
        ''' <remarks></remarks>
        Public Shared Sub HandleSwapPracticeLanguage(ByRef dtSource As DataTable, ByVal strEngFieldName As String, ByVal strChiFieldName As String)

            ' Retrieve SystemParameter HCSP
            Dim strPara As String = String.Empty
            Dim strDummy As String = String.Empty

            Dim udtCommFunctBLL As New Common.ComFunction.GeneralFunction()
            udtCommFunctBLL.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strPara, strDummy)

            ' Handle Language Swap
            If dtSource.Columns.Contains(strEngFieldName) AndAlso dtSource.Columns.Contains(strChiFieldName) Then
                For Each drRow As DataRow In dtSource.Rows
                    If strPara.Trim().ToUpper = strYes Then
                        ' On: Display Chinese Name if Contain Chinese Name, Else Display Eng Name
                        If drRow.IsNull(strChiFieldName) OrElse drRow(strChiFieldName).ToString().Trim() = "" Then
                            drRow(strChiFieldName) = drRow(strEngFieldName)
                        End If
                    Else
                        ' Off: Display Eng Name Only
                        drRow(strChiFieldName) = drRow(strEngFieldName)
                    End If
                Next

                ' To Reduce the DataTable/DataRow Status Information
                dtSource.AcceptChanges()
            End If
        End Sub

        ''' <summary>
        ''' [Static] Concate Display Column to DataTable according to Practice Display Type
        ''' </summary>
        ''' <param name="dtSource"></param>
        ''' <param name="enumPracticeDisplayType"></param>
        ''' <remarks></remarks>
        Public Shared Sub ConcatePracticeDisplayColumn(ByRef dtSource As DataTable, ByVal enumPracticeDisplayType As PracticeDisplayType)

            Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter()

            ' Display Column for UI
            dtSource.Columns.Add(PracticeDisplayField.Display_Eng, GetType(String))
            dtSource.Columns.Add(PracticeDisplayField.Display_Chi, GetType(String))

            Dim udtAddress As AddressModel = Nothing

            For Each drRow As DataRow In dtSource.Rows
                Select Case enumPracticeDisplayType

                    Case PracticeDisplayType.Practice
                        ' Practice Name
                        drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") "
                        drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") "
                    Case PracticeDisplayType.PracticeBankAccount
                        ' Practice Name (Practice ID) [Mask Bank Account Num]
                        drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.maskBankAccount(drRow(tablePracticeBank.Bank_Account_No).ToString().Trim()) + "]"
                        drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.maskBankAccount(drRow(tablePracticeBank.Bank_Account_No).ToString().Trim()) + "]"
                    Case PracticeDisplayType.PracticeAddress
                        ' Practice Name (Practice ID) [Format Practice Address]
                        udtAddress = New AddressModel(drRow("Room").ToString(), drRow("Floor").ToString(), drRow("Block").ToString(), drRow("Building").ToString(), drRow("Building_Chi"), drRow("District").ToString(), Nothing)
                        With udtAddress
                            drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.formatAddress(.Room, .Floor, .Block, .Building, .District, .AreaCode) + "]"
                            drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.formatAddressChi(.Room, .Floor, .Block, .ChiBuilding, .District, .AreaCode) + "]"
                        End With
                End Select
            Next

            dtSource.AcceptChanges()

        End Sub

#End Region

#Region "Internal Class"

        Public Enum PracticeDisplayType
            Practice
            PracticeBankAccount
            PracticeAddress
        End Enum

        Public Class PracticeDisplayField
            Public Const Display_Eng As String = "Display_Name"
            Public Const Display_Chi As String = "Display_Name_Chi"
        End Class

        Private Class tablePracticeBank

            Public Const Practice_Name As String = "Practice_Name"
            Public Const Practice_Name_Chi As String = "Practice_Name_Chi"

            Public Const Building As String = "Building"
            Public Const Building_Chi As String = "Building_Chi"

            Public Const PracticeID As String = "PracticeID"
            Public Const Bank_Account_No As String = "Bank_Account_No"

        End Class
#End Region

#Region "Checking Function"

        ''' <summary>
        ''' Check if the sp contain Active Bank Account
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkActivePracticeBankAcct(ByVal strSPID As String) As Boolean
            Dim blnRes As Boolean = False
            Dim udtDB As Database = New Database
            Dim dtRes As DataTable = New DataTable
            Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_PracticeBankAccountActive_get_bySPID", prams, dtRes)
            Try
                If dtRes.Rows.Count > 0 Then
                    blnRes = True
                Else
                    blnRes = False
                End If
            Catch eSQL As SqlException
                blnRes = False
                Throw eSQL
            Catch ex As Exception
                blnRes = False
                Throw ex
            End Try
            Return blnRes
        End Function
#End Region

#Region "Retrieve Function"

        Public Function getActivePracticeWithAvailableScheme(ByVal strSPID As String, ByVal strDataEntry As String, ByVal udtPracticeList As Practice.PracticeModelCollection, ByVal udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection) As PracticeDisplayModelCollection
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
            Dim udtResPracticeDisplayList As PracticeDisplayModelCollection = New PracticeDisplayModelCollection()

            '-----------------------------------------------------------------------
            '1. Retrieve all Active Practice (Display) List (Practice Status Only)
            '-----------------------------------------------------------------------
            Dim udtPracticeDisplayList As PracticeDisplayModelCollection = Me.getActivePractice(strSPID, strDataEntry)

            '-----------------------------------------------------------------------
            '2. Filter with the Practice (Display), if it contain available scheme (Valid Claim Period and ServiceFee Checking)
            '-----------------------------------------------------------------------
            Dim intPracticeDisplaySeq As Integer = 0

            For Each udtPracticeDisplayModel As PracticeDisplayModel In udtPracticeDisplayList
                intPracticeDisplaySeq = 0

                For Each udtPractice As Practice.PracticeModel In udtPracticeList.Values
                    If udtPracticeDisplayModel.PracticeID = udtPractice.DisplaySeq Then
                        intPracticeDisplaySeq = udtPractice.DisplaySeq
                    End If
                Next

                If intPracticeDisplaySeq <> 0 Then
                    If udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtPracticeList(intPracticeDisplaySeq).PracticeSchemeInfoList, udtSchemeInfoList).Count > 0 Then
                        Dim udtPractice As PracticeModel = udtPracticeList(udtPracticeDisplayModel.PracticeID)

                        If IsNothing(udtPractice) Then Continue For

                        Dim udtPracticeSchemeInfoList As New PracticeSchemeInfo.PracticeSchemeInfoModelCollection

                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                            udtPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(udtPracticeScheme))
                        Next

                        '----------------------------------------
                        '2.1 Filter only active practice scheme
                        '----------------------------------------
                        'Dim blnContainActiveScheme As Boolean = False

                        'For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        '    If udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                        '            udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist OrElse _
                        '            udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then
                        '        blnContainActiveScheme = True
                        '        Exit For
                        '    End If
                        'Next

                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                            If udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.Active AndAlso _
                                    udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist AndAlso _
                                    udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then

                                If Not udtPracticeSchemeInfoList.Filter(udtPracticeScheme.SchemeCode, udtPracticeScheme.SubsidizeCode) Is Nothing Then
                                    udtPracticeSchemeInfoList.Remove(udtPracticeScheme)
                                End If

                            End If
                        Next

                        'If blnContainActiveScheme = False Then Continue For
                        If udtPracticeSchemeInfoList.Count = 0 Then Continue For

                        '----------------------------------------
                        '2.2 Filter practice scheme
                        '----------------------------------------

                        ' CRE20-023-71 (COVID19 - Medical Exemption Record) [Start][Winnie SUEN]
                        ' -----------------------------------------------------------------------
                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values

                            Dim blnRemove As Boolean = False

                            '2.2.1 readonly in claim
                            If udtSchemeClaimBLL.CheckSchemeClaimReadonly(udtPracticeScheme) Then
                                blnRemove = True
                            End If

                            '2.2.2 Not allow to claim for Data Entry
                            If udtSchemeClaimBLL.CheckSchemeClaimAllowDataEntryClaim(udtPracticeScheme) = False Then
                                blnRemove = True
                            End If

                            If blnRemove Then
                                If Not udtPracticeSchemeInfoList.Filter(udtPracticeScheme.SchemeCode, udtPracticeScheme.SubsidizeCode) Is Nothing Then
                                    udtPracticeSchemeInfoList.Remove(udtPracticeScheme)
                                End If
                            End If

                        Next
                        ' CRE20-023-71 (COVID19 - Medical Exemption Record) [End][Winnie SUEN]

                        If udtPracticeSchemeInfoList.Count = 0 Then Continue For

                        '----------------------------------------
                        ' Add available practice scheme in list
                        '----------------------------------------
                        udtResPracticeDisplayList.Add(udtPracticeDisplayModel)

                    End If
                End If

            Next

            Return udtResPracticeDisplayList

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Function

        ''' <summary>
        ''' Get Active Practice of a SP that has AvailableScheme For Claim
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getActivePracticeWithAvailableScheme(ByVal strSPID As String, ByVal udtPracticeList As Practice.PracticeModelCollection, ByVal udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection) As PracticeDisplayModelCollection
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()
            Dim udtResPracticeDisplayList As PracticeDisplayModelCollection = New PracticeDisplayModelCollection()

            '-----------------------------------------------------------------------
            '1. Retrieve all Active Practice (Display) List (Practice Status Only)
            '-----------------------------------------------------------------------
            Dim udtPracticeDisplayList As PracticeDisplayModelCollection = Me.getActivePractice(strSPID)

            '-----------------------------------------------------------------------
            '2. Filter with the Practice (Display), if it contain available scheme (Valid Claim Period and ServiceFee Checking)
            '-----------------------------------------------------------------------
            For Each udtPracticeDisplayModel As PracticeDisplayModel In udtPracticeDisplayList

                Dim udtPractice As PracticeModel = udtPracticeList(udtPracticeDisplayModel.PracticeID)
                If IsNothing(udtPractice) Then Continue For

                If udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtPractice.PracticeSchemeInfoList, udtSchemeInfoList).Count > 0 Then
                    Dim udtPracticeSchemeInfoList As New PracticeSchemeInfo.PracticeSchemeInfoModelCollection

                    For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        udtPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(udtPracticeScheme))
                    Next

                    '----------------------------------------
                    '2.1 Filter only active practice scheme
                    '----------------------------------------
                    'Dim blnContainActiveScheme As Boolean = False

                    'For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    '    If udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                    '            udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist OrElse _
                    '            udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then
                    '        blnContainActiveScheme = True
                    '        Exit For
                    '    End If
                    'Next

                    For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        If udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.Active AndAlso _
                                udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist AndAlso _
                                udtPracticeScheme.RecordStatus <> PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then

                            If Not udtPracticeSchemeInfoList.Filter(udtPracticeScheme.SchemeCode, udtPracticeScheme.SubsidizeCode) Is Nothing Then
                                udtPracticeSchemeInfoList.Remove(udtPracticeScheme)
                            End If

                        End If
                    Next

                    'If Not blnContainActiveScheme Then Continue For
                    If udtPracticeSchemeInfoList.Count = 0 Then Continue For

                    '----------------------------------------
                    '2.2 Filter practice scheme without readonly in claim
                    '----------------------------------------
                    'Dim blnContainReadonlyScheme As Boolean = True

                    'For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    '    If Not udtSchemeClaimBLL.CheckSchemeClaimReadonly(udtPracticeSchemeInfo) Then
                    '        blnContainReadonlyScheme = False
                    '        Exit For
                    '    End If
                    'Next

                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        If udtSchemeClaimBLL.CheckSchemeClaimReadonly(udtPracticeSchemeInfo) Then

                            If Not udtPracticeSchemeInfoList.Filter(udtPracticeSchemeInfo.SchemeCode, udtPracticeSchemeInfo.SubsidizeCode) Is Nothing Then
                                udtPracticeSchemeInfoList.Remove(udtPracticeSchemeInfo)
                            End If

                        End If
                    Next

                    'If blnContainReadonlyScheme Then Continue For
                    If udtPracticeSchemeInfoList.Count = 0 Then Continue For

                    '----------------------------------------
                    ' Add available practice scheme in list
                    '----------------------------------------
                    udtResPracticeDisplayList.Add(udtPracticeDisplayModel)

                End If

            Next

            Return udtResPracticeDisplayList

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Function

        ''' <summary>
        ''' Retrieve Active Practice Display List (By SPID)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getActivePractice(ByVal strSPID As String) As PracticeDisplayModelCollection

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawActivePracticeBankAcct(strSPID)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            Return Me.convertPractice(dtPractice)

        End Function

        ''' <summary>
        ''' Retrieve Active Practice Display List (By SPID + DataEntry)
        ''' (Assigned Practice to DataEntry)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getActivePractice(ByVal strSPID As String, ByVal strDataEntry As String) As PracticeDisplayModelCollection

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawActivePracticeBankAcct(strSPID, strDataEntry)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            Return Me.convertPractice(dtPractice)
        End Function

        ''' <summary>
        ''' Retrieve Active Practice For Display (By SPID + Practice Display Type)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="enumPracticeDisplayType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getActivePractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As PracticeDisplayType) As DataTable

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawActivePracticeBankAcct(strSPID)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            ConcatePracticeDisplayColumn(dtPractice, enumPracticeDisplayType)

            Return dtPractice
        End Function

        ''' <summary>
        ''' Retrieve Active Practice For Display (By SPID + DataEntry + Practice Display Type)
        ''' (Assigned Practice to DataEntry)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <param name="enumPracticeDisplayType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getActivePractice(ByVal strSPID As String, ByVal strDataEntry As String, ByVal enumPracticeDisplayType As PracticeDisplayType) As DataTable

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawActivePracticeBankAcct(strSPID, strDataEntry)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            ConcatePracticeDisplayColumn(dtPractice, enumPracticeDisplayType)

            Return dtPractice
        End Function

        ''' <summary>
        ''' Retrieve All Practice For Display (By SPID + Practice Display Type)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="enumPracticeDisplayType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllPractice(ByVal strSPID As String, ByVal enumPracticeDisplayType As PracticeDisplayType) As DataTable

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawAllPracticeBankAcct(strSPID)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            ConcatePracticeDisplayColumn(dtPractice, enumPracticeDisplayType)

            Return dtPractice
        End Function

        ''' <summary>
        ''' Retrieve All Practice For Display (By SPID + DataEntry + Practice Display Type)
        ''' (Assigned Practice to DataEntry)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <param name="enumPracticeDisplayType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllPractice(ByVal strSPID As String, ByVal strDataEntry As String, ByVal enumPracticeDisplayType As PracticeDisplayType) As DataTable

            ' Get Practice Information
            Dim dtPractice As DataTable = Me.getRawAllPracticeBankAcct(strSPID, strDataEntry)

            ' Handle Swap the Practice Eng & Chi Information
            HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Practice_Name, tablePracticeBank.Practice_Name_Chi)
            'HandleSwapPracticeLanguage(dtPractice, tablePracticeBank.Building, tablePracticeBank.Building_Chi)

            ConcatePracticeDisplayColumn(dtPractice, enumPracticeDisplayType)

            Return dtPractice
        End Function

#End Region

#Region "[Private] Function"

        ' Retrieve Function

        ''' <summary>
        ''' Retrieve Raw All PracticeBankAccount DataTable (By SPID + DataEntry)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRawAllPracticeBankAcct(ByVal strSPID As String, ByVal strDataEntry As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database()

            Dim dtPracticeBank As New DataTable()
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID), _
                    udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntry)}
                udtDB.RunProc("proc_PracticeBankAccountAll_get_bySPIDDEID", prams, dtPracticeBank)

            Catch eSQL As SqlException
                dtPracticeBank = Nothing
                Throw eSQL
            Catch ex As Exception
                dtPracticeBank = Nothing
                Throw ex
            End Try
            Return dtPracticeBank

        End Function

        ''' <summary>
        ''' Retrieve Raw All PracticeBankAccount DataTable (By SPID)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRawAllPracticeBankAcct(ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            Dim dtPracticeBank As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccountAll_get_bySPID", prams, dtPracticeBank)


            Catch eSQL As SqlException
                dtPracticeBank = Nothing
                Throw eSQL
            Catch ex As Exception
                dtPracticeBank = Nothing
                Throw ex
            End Try
            Return dtPracticeBank
        End Function

        ''' <summary>
        ''' Retrieve Raw Active PracticeBankAccount DataTable (By SPID + DataEntry)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="strDataEntry"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRawActivePracticeBankAcct(ByVal strSPID As String, ByVal strDataEntry As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database()

            Dim dtPracticeBank As New DataTable()
            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID), _
                    udtDB.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntry)}
                udtDB.RunProc("proc_PracticeBankAccountActive_get_bySPIDDEID", prams, dtPracticeBank)

            Catch eSQL As SqlException
                dtPracticeBank = Nothing
                Throw eSQL
            Catch ex As Exception
                dtPracticeBank = Nothing
                Throw ex
            End Try
            Return dtPracticeBank

        End Function

        ''' <summary>
        ''' Retrieve Raw Active PracticeBankAccount DataTable (By SPID)
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getRawActivePracticeBankAcct(ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            Dim dtPracticeBank As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccountActive_get_bySPID", prams, dtPracticeBank)


            Catch eSQL As SqlException
                dtPracticeBank = Nothing
                Throw eSQL
            Catch ex As Exception
                dtPracticeBank = Nothing
                Throw ex
            End Try
            Return dtPracticeBank
        End Function

        ' Convert Function

        ''' <summary>
        ''' Convert Practice Display DataTable to PracticeDisplayModelCollection
        ''' </summary>
        ''' <param name="dtSource"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function convertPractice(ByVal dtSource As DataTable) As PracticeDisplayModelCollection

            ' Retrieve SystemParameter HCSP
            Dim strPara As String = String.Empty
            Dim strDummy As String = String.Empty

            Dim udtCommFunctBLL As New Common.ComFunction.GeneralFunction()
            udtCommFunctBLL.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strPara, strDummy)

            Dim udtPracticeDisplayModelList As New PracticeDisplayModelCollection()

            For Each drRow As DataRow In dtSource.Rows
                Dim udtPracticeDisplayMode As New PracticeDisplayModel(drRow)

                If strPara.Trim().ToUpper = strYes Then
                    udtPracticeDisplayMode.DisplayEngOnly = False
                    If udtPracticeDisplayMode.PracticeName.Trim() <> udtPracticeDisplayMode.PracticeNameChi.Trim() Then
                        udtPracticeDisplayMode.PracticeNameDisplayChi = True
                    End If
                Else
                    udtPracticeDisplayMode.DisplayEngOnly = True
                End If
                udtPracticeDisplayModelList.Add(udtPracticeDisplayMode)
            Next

            Return udtPracticeDisplayModelList
        End Function
#End Region

    End Class

#Region "Model Class"

    <Serializable()> Partial Public Class PracticeDisplayModel

#Region "Private Member"

        Private _strSP_ID As String
        Private _intPracticeID As Integer
        Private _strPractice_Name As String
        Private _strPractice_Name_Chi As String
        Private _strService_Category_Code As String

        Private _strPractice_Status As String
        Private _intBankAcctID As Integer
        Private _strBank_Account_No As String
        Private _strBank_Acc_Holder As String
        Private _strBankAcct_Status As String

        Private _strBankAccountKey As String
        Private _strRoom As String
        Private _strFloor As String
        Private _strBlock As String
        Private _strBuilding As String

        Private _strBuilding_Chi As String
        Private _strDistrict As String
        Private _intAddress_Code As Nullable(Of Integer)


        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        Private _udtProfession As Profession.ProfessionModel
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

#End Region

#Region "Property"

        Public Property SPID() As String
            Get
                Return Me._strSP_ID
            End Get
            Set(ByVal value As String)
                Me._strSP_ID = value
            End Set
        End Property

        Public Property PracticeID() As Integer
            Get
                Return Me._intPracticeID
            End Get
            Set(ByVal value As Integer)
                Me._intPracticeID = value
            End Set
        End Property

        Public Property PracticeName() As String
            Get
                Return Me._strPractice_Name
            End Get
            Set(ByVal value As String)
                Me._strPractice_Name = value
            End Set
        End Property

        Public Property PracticeNameChi() As String
            Get
                Return Me._strPractice_Name_Chi
            End Get
            Set(ByVal value As String)
                Me._strPractice_Name_Chi = value
            End Set
        End Property

        Public Property ServiceCategoryCode() As String
            Get
                Return Me._strService_Category_Code
            End Get
            Set(ByVal value As String)
                Me._strService_Category_Code = value
            End Set
        End Property

        Public Property PracticeStatus() As String
            Get
                Return Me._strPractice_Status
            End Get
            Set(ByVal value As String)
                Me._strPractice_Status = value
            End Set
        End Property

        Public Property BankAcctID() As Integer
            Get
                Return Me._intBankAcctID
            End Get
            Set(ByVal value As Integer)
                Me._intBankAcctID = value
            End Set
        End Property

        Public Property BankAccountNo() As String
            Get
                Return Me._strBank_Account_No
            End Get
            Set(ByVal value As String)
                Me._strBank_Account_No = value
            End Set
        End Property

        Public Property BankAccHolder() As String
            Get
                Return Me._strBank_Acc_Holder
            End Get
            Set(ByVal value As String)
                Me._strBank_Acc_Holder = value
            End Set
        End Property

        Public Property BankAcctStatus() As String
            Get
                Return Me._strBankAcct_Status
            End Get
            Set(ByVal value As String)
                Me._strBankAcct_Status = value
            End Set
        End Property

        Public Property BankAccountKey() As String
            Get
                Return Me._strBankAccountKey
            End Get
            Set(ByVal value As String)
                Me._strBankAccountKey = value
            End Set
        End Property

        Public Property Room() As String
            Get
                Return Me._strRoom
            End Get
            Set(ByVal value As String)
                Me._strRoom = value
            End Set
        End Property

        Public Property Floor() As String
            Get
                Return Me._strFloor
            End Get
            Set(ByVal value As String)
                Me._strFloor = value
            End Set
        End Property

        Public Property Block() As String
            Get
                Return Me._strBlock
            End Get
            Set(ByVal value As String)
                Me._strBlock = value
            End Set
        End Property

        Public Property Building() As String
            Get
                Return Me._strBuilding
            End Get
            Set(ByVal value As String)
                Me._strBuilding = value
            End Set
        End Property

        Public Property BuildingChi() As String
            Get
                Return Me._strBuilding_Chi
            End Get
            Set(ByVal value As String)
                Me._strBuilding_Chi = value
            End Set
        End Property

        Public Property District() As String
            Get
                Return Me._strDistrict
            End Get
            Set(ByVal value As String)
                Me._strDistrict = value
            End Set
        End Property

        Public Property AddressCode() As Nullable(Of Integer)
            Get
                Return Me._intAddress_Code
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAddress_Code = value
            End Set
        End Property

        ' Addition Field

        Private _blnDisplayEngOnly As Boolean = False
        ' Indicate the Mirgration Complete Setting is turn off or not: Turn off: Display Eng Only = True
        Public Property DisplayEngOnly() As Boolean
            Get
                Return Me._blnDisplayEngOnly
            End Get
            Set(ByVal value As Boolean)
                Me._blnDisplayEngOnly = value
            End Set
        End Property

        Private _blnPracticeNameDisplayChi As Boolean = False
        ' Indicate the Current Display is Chinese Description or not
        Public Property PracticeNameDisplayChi() As Boolean
            Get
                Return Me._blnPracticeNameDisplayChi
            End Get
            Set(ByVal value As Boolean)
                Me._blnPracticeNameDisplayChi = value
            End Set
        End Property

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        Public ReadOnly Property Profession() As Profession.ProfessionModel
            Get
                If Me._udtProfession Is Nothing Then
                    _udtProfession = (New Profession.ProfessionBLL).GetProfessionListByServiceCategoryCode(Me._strService_Category_Code)
                End If

                Return _udtProfession
            End Get
        End Property
        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strRegistrationCode As String = String.Empty
        Public Property RegistrationCode() As String
            Get
                Return _strRegistrationCode
            End Get
            Set(ByVal value As String)
                _strRegistrationCode = UCase(value).Trim
            End Set
        End Property

        Private _strProvideDHCService As String = String.Empty
        Public ReadOnly Property ProvideDHCService() As String
            Get
                If String.IsNullOrEmpty(_strProvideDHCService) Then
                    Dim udtProfessionalBLL As New Professional.ProfessionalBLL

                    If udtProfessionalBLL.CheckDHCSPMapping(Me.ServiceCategoryCode, Me.RegistrationCode) Then
                        _strProvideDHCService = YesNo.Yes
                    Else
                        _strProvideDHCService = YesNo.No
                    End If
                End If

                Return _strProvideDHCService
            End Get
        End Property
        ' CRE19-006 (DHC) [End][Winnie]
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Sub New(ByVal drPracticeBank As DataRow)
            Me._strSP_ID = drPracticeBank("SP_ID").ToString().Trim()
            Me._intPracticeID = CInt(drPracticeBank("PracticeID"))
            Me._strPractice_Name = drPracticeBank("Practice_Name").ToString().Trim()
            Me._strPractice_Name_Chi = drPracticeBank("Practice_Name_Chi").ToString().Trim()
            Me._strService_Category_Code = drPracticeBank("Service_Category_Code").ToString().Trim()

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strRegistrationCode = drPracticeBank("Registration_Code").ToString().Trim()
            ' CRE19-006 (DHC) [End][Winnie]

            Me._strPractice_Status = drPracticeBank("Practice_Status").ToString().Trim()
            Me._intBankAcctID = drPracticeBank("BankAcctID").ToString().Trim()
            Me._strBank_Account_No = drPracticeBank("Bank_Account_No").ToString().Trim()
            Me._strBank_Acc_Holder = drPracticeBank("Bank_Acc_Holder").ToString().Trim()
            Me._strBankAcct_Status = drPracticeBank("BankAcct_Status").ToString().Trim()

            Me._strBankAccountKey = drPracticeBank("BankAccountKey").ToString().Trim()


            If drPracticeBank.IsNull("Room") Then
                Me._strRoom = String.Empty
            Else
                Me._strRoom = drPracticeBank("Room").ToString().Trim()
            End If

            If drPracticeBank.IsNull("Floor") Then
                Me._strFloor = String.Empty
            Else
                Me._strFloor = drPracticeBank("Floor").ToString().Trim()
            End If

            If drPracticeBank.IsNull("Block") Then
                Me._strBlock = String.Empty
            Else
                Me._strBlock = drPracticeBank("Block").ToString().Trim()
            End If

            If drPracticeBank.IsNull("Building") Then
                Me._strBuilding = String.Empty
            Else
                Me._strBuilding = drPracticeBank("Building").ToString().Trim()
            End If

            If drPracticeBank.IsNull("Building_Chi") Then
                Me._strBuilding_Chi = String.Empty
            Else
                Me._strBuilding_Chi = drPracticeBank("Building_Chi").ToString().Trim()
            End If

            If drPracticeBank.IsNull("District") Then
                Me._strDistrict = String.Empty
            Else
                Me._strDistrict = drPracticeBank("District").ToString().Trim()
            End If


            If drPracticeBank.IsNull("Address_Code") Then
                Me._intAddress_Code = Nothing
            Else
                Me._intAddress_Code = CInt(drPracticeBank("Address_Code"))
            End If


        End Sub

#End Region

    End Class

    <Serializable()> Partial Public Class PracticeDisplayModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtPracticeDisplayModel As PracticeDisplayModel)
            MyBase.Add(udtPracticeDisplayModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtPracticeDisplayModel As PracticeDisplayModel)
            MyBase.Remove(udtPracticeDisplayModel)
        End Sub

        Public Function Filter(ByVal intPracticeID As Integer) As PracticeDisplayModel
            Dim returnPracticeDisplayModel As PracticeDisplayModel = Nothing
            For Each practiceDisplayModel As PracticeDisplayModel In Me
                If practiceDisplayModel.PracticeID = intPracticeID Then
                    returnPracticeDisplayModel = practiceDisplayModel
                    Exit For
                End If
            Next

            Return returnPracticeDisplayModel
        End Function

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Public Function FilterByTextOnlyAvailable(ByVal udtSP As ServiceProvider.ServiceProviderModel) As PracticeDisplayModelCollection
            Dim udtResPracticeDisplayList As PracticeDisplayModelCollection = New PracticeDisplayModelCollection()

            Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()

            Dim udtSchemeClaimList As Scheme.SchemeClaimModelCollection

            Dim blnTextOnlyAvailableByPractice As Boolean

            For Each udtPracticeDisplay As PracticeDisplayModel In Me

                blnTextOnlyAvailableByPractice = False

                udtSchemeClaimList = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtSP.PracticeList(udtPracticeDisplay.PracticeID).PracticeSchemeInfoList, udtSP.SchemeInfoList)
                For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeClaimList

                    If udtSchemeClaim.TextOnlyAvailable Then
                        blnTextOnlyAvailableByPractice = True
                        Exit For
                    End If

                Next

                If blnTextOnlyAvailableByPractice Then
                    udtResPracticeDisplayList.Add(udtPracticeDisplay)
                End If

            Next

            Return udtResPracticeDisplayList
        End Function
        ' CRE13-001 - EHAPP [End][Tommy L]

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As PracticeDisplayModel
            Get
                Return CType(MyBase.Item(intIndex), PracticeDisplayModel)
            End Get
        End Property

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check any practice available for claim (1. Profession claim period)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasPracticeAvailableForClaim() As Boolean
            For Each obj As PracticeDisplayModel In Me
                If obj.Profession.IsClaimPeriod Then Return True
            Next
        End Function

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function FilterByActiveScheme(udtPracticeList As PracticeModelCollection, blnIncludeActiveOnly As Boolean) As ArrayList
            Dim udtPracticeDisplayList As New PracticeDisplayModelCollection

            For Each udtPracticeDisplay As PracticeDisplayModel In Me
                Dim udtPractice As PracticeModel = udtPracticeList(udtPracticeDisplay.PracticeID)

                If IsNothing(udtPractice) Then Continue For

                If blnIncludeActiveOnly Then
                    ' Filter only active practice scheme
                    Dim blnContainActiveScheme As Boolean = False

                    For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        If udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                                udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist OrElse _
                                udtPracticeScheme.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then
                            blnContainActiveScheme = True
                            Exit For
                        End If
                    Next

                    If blnContainActiveScheme = False Then Continue For

                End If

                udtPracticeDisplayList.Add(udtPracticeDisplay)

            Next

            Return udtPracticeDisplayList

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Class
#End Region
End Namespace
