Imports Common.ComFunction
Imports Common.Component
Imports System.Web.Mvc
Imports System.Xml

Namespace Controllers
    <Localization>
    Public Class StaticController
        Inherits BaseController
        Function PrivacyPolicy() As ActionResult
            'Dim arra = New Integer() {1, 2, 3}
            'Dim a = arra(6)
            Return View()
        End Function

        Function ImportantNotice() As ActionResult
            Return View()
        End Function

        Function TextSize() As ActionResult
            Return View()
        End Function

        Function SystemMaintenance() As ActionResult
            Dim viewModel As SystemMaintenanceViewModel = New SystemMaintenanceViewModel
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            viewModel.MonthlyModelList = GetRegularMaint(strLang)
            viewModel.UrgentLyModelList = GetUrgentMaint(strLang)
            Return View(viewModel)
        End Function
        Private Function GetRegularMaint(strLang As String) As List(Of SystemMaintenanceModel)
            Dim modelList As List(Of SystemMaintenanceModel) = New List(Of SystemMaintenanceModel)
            Dim XmlFilePath As String
            If XMLMain.DBLink Then
                Dim udtGeneralFunction As New GeneralFunction
                XmlFilePath = String.Format("{0}{1}", udtGeneralFunction.getSystemParameter("MaintenanceXMLPath"), "RegularMaint.xml")
            Else
                XmlFilePath = Server.MapPath("~/XMLData/RegularMaint.xml")
            End If
            'Get XML Data
            Dim xmlDoc As New XmlDocument()
            Dim xpath As String = "//Schedule"
            Dim ExpiredDateTime As DateTime
            Dim rawExpiredDTM As String
            xmlDoc.Load(XmlFilePath)
            Dim nodes As XmlNodeList = xmlDoc.SelectNodes(xpath)
            For Each node As XmlNode In nodes
                rawExpiredDTM = node("expiry_dtm").InnerText
                ExpiredDateTime = Convert.ToDateTime(rawExpiredDTM)
                If (DateTime.Now <= ExpiredDateTime) Then
                    If strLang.ToLower().Equals(CultureLanguage.English) Then
                        modelList.Add(New SystemMaintenanceModel With {
                                      .ShowDate = node("EN_Date").InnerText,
                                      .ShowTime = node("EN_Time").InnerText,
                                      .SID = node("s_id").InnerText,
                                      .Type = node("Type").InnerText
                                  })
                    End If
                    If strLang.ToLower().Equals(CultureLanguage.TradChinese) Then
                        modelList.Add(New SystemMaintenanceModel With {
                                      .ShowDate = node("ZH_Date").InnerText,
                                      .ShowTime = node("ZH_Time").InnerText,
                                      .SID = node("s_id").InnerText,
                                      .Type = node("Type").InnerText
                                  })
                    End If
                End If
            Next
            Return modelList
        End Function

        Private Function GetUrgentMaint(strLang As String) As List(Of SystemMaintenanceModel)
            Dim modelList As List(Of SystemMaintenanceModel) = New List(Of SystemMaintenanceModel)

            Dim XmlFilePath As String
            If XMLMain.DBLink Then
                Dim udtGeneralFunction As New GeneralFunction
                XmlFilePath = String.Format("{0}{1}", udtGeneralFunction.getSystemParameter("MaintenanceXMLPath"), "UrgentMaint.xml")
            Else
                XmlFilePath = Server.MapPath("~/XMLData/UrgentMaint.xml")
            End If

            'Get XML Data
            Dim xmlDoc As New XmlDocument()
            Dim xpath As String = "//Schedule"
            Dim ExpiredDateTime As DateTime
            Dim rawExpiredDTM As String
            xmlDoc.Load(XmlFilePath)
            Dim nodes As XmlNodeList = xmlDoc.SelectNodes(xpath)
            For Each node As XmlNode In nodes
                rawExpiredDTM = node("expiry_dtm").InnerText
                ExpiredDateTime = Convert.ToDateTime(rawExpiredDTM)
                If (DateTime.Now <= ExpiredDateTime) Then
                    If strLang.ToLower().Equals(CultureLanguage.English) Then
                        modelList.Add(New SystemMaintenanceModel With {
                                      .ShowDate = node("EN_Date").InnerText,
                                      .ShowTime = node("EN_Time").InnerText,
                                      .SID = node("s_id").InnerText,
                                      .Type = node("Type").InnerText
                                  })
                    End If
                    If strLang.ToLower().Equals(CultureLanguage.TradChinese) Then
                        modelList.Add(New SystemMaintenanceModel With {
                                      .ShowDate = node("ZH_Date").InnerText,
                                      .ShowTime = node("ZH_Time").InnerText,
                                      .SID = node("s_id").InnerText,
                                      .Type = node("Type").InnerText
                                  })
                    End If
                End If
            Next
            Return modelList
        End Function

    End Class
End Namespace