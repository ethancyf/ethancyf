Imports Common.ComFunction.ParameterFunction
Imports Common.Component.FileGeneration

Partial Public Class StatisticsResultBase
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal udtParameterList As ParameterCollection)
        phStatisticsResult.Controls.Clear()

        Dim intCount As Integer = 0

        For Each udtParameter As ParameterObject In udtParameterList
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim listParam As ParameterObjectList = DirectCast(udtParameter, ParameterObjectList)

                Dim strChkBoxItemString As String = "<ul style='padding-left: 15px; margin: 0px'>"
                For Each strValue As String In listParam.ParamValueList
                    strChkBoxItemString += "<li>" + strValue + "</li>"
                Next
                strChkBoxItemString += "</ul>"

                Dim con As Control = LoadControl("StatisticsResult.ascx")
                intCount += 1
                con.ID = "UCSRB_" + intCount.ToString()
                phStatisticsResult.Controls.Add(con)
                CType(con, StatisticsResult).SetDisplayText(udtParameter.ParamName, strChkBoxItemString)

            ElseIf TypeOf udtParameter Is ParameterObjectWithLegend Then
                Dim paraObjectWithLegend As ParameterObjectWithLegend = DirectCast(udtParameter, ParameterObjectWithLegend)

                Dim con As Control = LoadControl("StatisticsResultWithLegend.ascx")
                intCount += 1
                con.ID = "UCSRB_" + intCount.ToString()
                phStatisticsResult.Controls.Add(con)
                CType(con, StatisticsResultWithLegend).SetDisplayText(udtParameter.ParamName, udtParameter.ParamValue)
                CType(con, StatisticsResultWithLegend).SetInfoBtnOn(paraObjectWithLegend.ParamLegendType)

            ElseIf TypeOf udtParameter Is ParameterObject Then
                Dim con As Control = LoadControl("StatisticsResult.ascx")
                intCount += 1
                con.ID = "UCSRB_" + intCount.ToString()
                phStatisticsResult.Controls.Add(con)
                CType(con, StatisticsResult).SetDisplayText(udtParameter.ParamName, udtParameter.ParamValue)
            End If

        Next
    End Sub

    Public Sub Build(ByVal strResultTitle As String, ByVal strResultGenTime As String)
        Dim con As Control = LoadControl("StatisticsResult.ascx")
        con.ID = "UCSRB_GenGenGenGenTime"
        phStatisticsResult.Controls.Add(con)
        CType(con, StatisticsResult).SetDisplayText(strResultTitle, strResultGenTime)
    End Sub

    Public Sub Clear()
        Me.phStatisticsResult.Controls.Clear()
    End Sub

End Class