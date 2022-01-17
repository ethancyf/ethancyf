Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.Scheme
Imports System.IO
Imports System.Xml.Serialization

Public Class SmartIDDummyCase

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Add IdeasVersion
    Public Shared Function GetDummyEHSAccount(ByVal udtScheme As SchemeClaimModel, ByVal eIdeasVersion As BLL.IdeasBLL.EnumIdeasVersion) As EHSAccountModel
        Return GetDummyEHSAccount(udtScheme.SchemeCode, eIdeasVersion)
    End Function
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Add IdeasVersion
    Public Shared Function GetDummyEHSAccount(ByVal strScheme As String, ByVal eIdeasVersion As BLL.IdeasBLL.EnumIdeasVersion) As EHSAccountModel
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        Dim udtPersInfo As New EHSPersonalInformationModel

        Dim sr As New StreamReader(ConfigurationManager.AppSettings("HKIDTestCaseDataFilePath"))
        Dim x As New XmlSerializer(udtPersInfo.GetType)

        udtPersInfo = x.Deserialize(sr)
        sr.Close()

        udtPersInfo.VoucherAccID = String.Empty
        udtPersInfo.DocCode = DocTypeCode.HKIC
        udtPersInfo.DateofIssue = udtPersInfo.UpdateDtm
        udtPersInfo.UpdateDtm = Nothing
        udtPersInfo.AdoptionPrefixNum = String.Empty
        udtPersInfo.SetDOBTypeSelected(False)
        udtPersInfo.CreateBySmartID = True        

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case eIdeasVersion
            Case BLL.IdeasBLL.EnumIdeasVersion.One
                udtPersInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS1
                udtPersInfo.Gender = String.Empty

            Case BLL.IdeasBLL.EnumIdeasVersion.Two
                udtPersInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS2
                udtPersInfo.Gender = String.Empty

            Case BLL.IdeasBLL.EnumIdeasVersion.TwoGender
                udtPersInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS2_WithGender

            Case BLL.IdeasBLL.EnumIdeasVersion.Combo
                udtPersInfo.Gender = String.Empty

            Case BLL.IdeasBLL.EnumIdeasVersion.ComboGender
                'Nothing to do
        End Select
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRE20-023-68 (Remove HA MingLiu) [Start][Winnie SUEN]
        ' -------------------------------------------------------------
        udtPersInfo.CName = BLL.VoucherAccountMaintenanceBLL.GetCName(udtPersInfo)

        If udtPersInfo.CName.Contains(" ") Then
            ' CCCode not exist in mapping
            udtPersInfo.CName = String.Empty
            udtPersInfo.CCCode1 = String.Empty
            udtPersInfo.CCCode2 = String.Empty
            udtPersInfo.CCCode3 = String.Empty
            udtPersInfo.CCCode4 = String.Empty
            udtPersInfo.CCCode5 = String.Empty
            udtPersInfo.CCCode6 = String.Empty
        End If
        ' CRE20-023-68 (Remove HA MingLiu) [End][Winnie SUEN]

        Dim udtEHSAccount As New EHSAccountModel()

        udtEHSAccount.SchemeCode = strScheme
        udtEHSAccount.EHSPersonalInformationList.Clear()
        udtEHSAccount.EHSPersonalInformationList.Add(udtPersInfo)

        udtEHSAccount.SetSearchDocCode(DocTypeCode.HKIC)

        Return udtEHSAccount

    End Function

End Class
