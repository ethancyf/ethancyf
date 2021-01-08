Imports Common.ComFunction.ParameterFunction
Imports Common.Component.FileGeneration

Partial Public Class reportCriteriaBase
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtUserControlList As FileGenerationUserControlModelCollection)
        phReportCriteria.Controls.Clear()

        For Each udtUserControl As FileGenerationUserControlModel In udtUserControlList.Values
            Dim con As Control = LoadControl(udtUserControl.UserControlID)
            con.ID = udtUserControl.DisplaySeq
            phReportCriteria.Controls.Add(con)
            'CRE13-003 Token Replacement [Start][Karl]
            CType(con, IReportCriteriaUC).Build(udtUserControl.UserControlSetting, udtUserControl.XmlSetting)
            'CType(con, IReportCriteriaUC).Build(udtUserControl.UserControlSetting)
            'CRE13-003 Token Replacement [End][Karl]
        Next

    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal udtUserControlList As FileGenerationUserControlModelCollection, ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        ' INT20-0055 (Fix concurrent browser submit in report submission) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If phReportCriteria.Controls.Count = 0 Then
            Throw New Exception("Error under concurrent browser.")
        End If
        ' INT20-0055 (Fix concurrent browser submit in report submission) [End][Chris YIM]

        For Each con As Control In phReportCriteria.Controls
            CType(con, IReportCriteriaUC).ValidateCriteriaInput(strReportID, udtUserControlList.Filter(con.ID).UserControlSetting, lstError, lstErrorParam1, lstErrorParam2)
        Next
    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of Common.ComObject.SystemMessage)
        Throw New NotImplementedException("ValidateCriteriaInput")
    End Function

    Public Function GetParameterString(ByVal udtUserControlList As FileGenerationUserControlModelCollection) As String
        Dim strParameterString As String = String.Empty

        For Each con As Control In phReportCriteria.Controls
            strParameterString += String.Format(", {0}", CType(con, IReportCriteriaUC).GetParameterString(udtUserControlList.Filter(con.ID).UserControlSetting))
        Next

        If strParameterString = String.Empty Then
            Return String.Empty
        Else
            Return strParameterString.Substring(2)
        End If

    End Function

    Public Function GetParameterList(ByVal udtUserControlList As FileGenerationUserControlModelCollection) As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        '[CRE13-003 Token Replacement [Start][Karl]
        'For Each con As Control In phReportCriteria.Controls
        '    For Each udtParameter As ParameterObject In CType(con, IReportCriteriaUC).GetParameterList(udtUserControlList.Filter(con.ID).UserControlSetting)
        '        udtParameterList.Add(udtParameter)
        '    Next
        'Next

        For Each con As Control In phReportCriteria.Controls
            For Each udtParameter As ParameterObject In CType(con, IReportCriteriaUC).GetParameterList(udtUserControlList.Filter(con.ID).UserControlSetting)
                If TypeOf udtParameter Is ParameterObjectList Then
                    Dim strList As String = String.Empty
                    Dim listParam As ParameterObjectList = DirectCast(udtParameter, ParameterObjectList)
                    For Each strValue As String In listParam.ParamValueList
                        If strList = String.Empty Then
                            strList = strValue
                        Else
                            strList += ", " + strValue
                        End If
                    Next

                    Dim udtParameterInside As New ParameterObject(udtParameter.ParamName, strList)

                    udtParameterList.Add(udtParameterInside)

                ElseIf TypeOf udtParameter Is ParameterObject Then
                    udtParameterList.Add(udtParameter)
                End If
            Next
        Next
        '[CRE13-003 Token Replacement [Start][End]

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput(ByVal udtUserControlList As FileGenerationUserControlModelCollection) As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        For Each con As Control In phReportCriteria.Controls
            Dim udtUserControl As FileGenerationUserControlModel = udtUserControlList.Filter(con.ID)

            For Each udtStoreProcParam As StoreProcParamObject In CType(con, IReportCriteriaUC).GetCriteriaInput(udtUserControl.UserControlSetting, udtUserControl.ParameterSuffix)
                udtStoreProcParamList.Add(udtStoreProcParam)
            Next
        Next

        Return udtStoreProcParamList

    End Function

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Sub Clear()
        Me.phReportCriteria.Controls.Clear()
    End Sub

    Public Function GetReportGenerationDate(ByVal udtUserControlList As FileGenerationUserControlModelCollection) As DateTime?
        Dim dtmReportGenDate As DateTime? = Nothing

        For Each con As Control In phReportCriteria.Controls
            Dim udtUserControl As FileGenerationUserControlModel = udtUserControlList.Filter(con.ID)
            Dim dtmCompareDate As DateTime? = CType(con, IReportCriteriaUC).GetReportGenerationDate(udtUserControl.UserControlSetting)

            If dtmCompareDate.HasValue Then

                If dtmCompareDate.HasValue Then
                    If dtmReportGenDate.HasValue Then
                        If dtmCompareDate.Value > dtmReportGenDate.Value Then
                            dtmReportGenDate = dtmCompareDate
                        End If
                    Else
                        dtmReportGenDate = dtmCompareDate
                    End If
                End If
            End If
        Next

        Return dtmReportGenDate
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
End Class