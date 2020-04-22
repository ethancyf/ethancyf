Imports System.Xml
Imports System.Web.UI.UserControl

Public Class Creator
    Inherits System.Web.UI.UserControl

#Region "Control tag"

    Public Class CriteriaControl
        Public Const ControlList As String = "ControlList"
        Public Const Control As String = "Control"
        Public Const CID As String = "CID"
        Public Const ClassName As String = "ClassName"
        Public Const Setting As String = "Setting"
        Public Const Field As String = "Field"
        Public Const DisplaySeq As String = "DisplaySeq"
        Public Const RemarkResource As String = "RemarkResource"
    End Class

    Public Class CriteriaSetting
        Public Const FID As String = "FID"
        Public Const DescResource As String = "DescResource"
        Public Const Visible As String = "Visible"
        Public Const DefaultValue As String = "DefaultValue"
        Public Const SPParamName As String = "SPParamName"
    End Class

    Public Class ResultSetup
        Public Const ResultList As String = "ResultList"
        Public Const ColumnList As String = "ColumnList"
        Public Const Column As String = "Column"
        Public Const RemarkResource As String = "RemarkResource"
    End Class

    Public Class ResultColumnSetting
        Public Const CName As String = "CName"
        Public Const DescResource As String = "DescResource"
        Public Const CWidth As String = "CWidth"
        Public Const ValueFormat As String = "ValueFormat"
    End Class

    Public Class ExportSetup
        Public Const ResultList As String = "ResultList"
        Public Const ColumnList As String = "ColumnList"
        Public Const Column As String = "Column"
        Public Const RemarkResource As String = "RemarkResource"
    End Class

    Public Class ExportColumnSetting
        Public Const CName As String = "CName"
        Public Const DescResource As String = "DescResource"
        Public Const CWidth As String = "CWidth"
        Public Const ValueFormat As String = "ValueFormat"
    End Class

#End Region

    Private _strControlPath As String = String.Empty

    Public Property ControlPath() As String
        Get
            Return _strControlPath
        End Get
        Set(ByVal value As String)
            _strControlPath = value
        End Set
    End Property

    Public Function StartTag(ByVal strTagName As String) As String
        Return "<" + strTagName + ">"
    End Function

    Public Function EndTag(ByVal strTagName As String) As String
        Return "</" + strTagName + ">"
    End Function

    Public Function CreateControl(ByVal xmlString As String) As List(Of Control)
        Dim xml As New XmlDocument
        Dim returnControlList As New List(Of Control)

        ' String to Xml format
        xml.LoadXml(xmlString.ToString.Trim)

        ' Get controls
        Dim controlList As XmlNode
        controlList = xml.FirstChild

        If controlList.HasChildNodes Then
            Dim xmlNodeList As XmlNodeList = controlList.ChildNodes

            ' Control node
            For Each listNode As XmlNode In xmlNodeList

                If listNode.Name = CriteriaControl.Control Then

                    Dim strControlID As String = String.Empty
                    Dim strControlName As String = String.Empty
                    ' dicBuildSetting -> to user control build method
                    Dim dicBuildSetting As New Dictionary(Of String, Dictionary(Of String, String))

                    For Each controlNode As XmlNode In listNode
                        If controlNode.Name = CriteriaControl.CID Then
                            strControlID = controlNode.InnerText
                        End If

                        If controlNode.Name = CriteriaControl.ClassName Then
                            strControlName = controlNode.InnerText
                        End If

                        If controlNode.Name = CriteriaControl.Setting Then
                            Dim fieldNodeList As XmlNodeList = controlNode.ChildNodes

                            For Each fieldNode As XmlNode In fieldNodeList
                                Dim strFieldID As String = String.Empty
                                Dim dicFieldSetting As New Dictionary(Of String, String)

                                If fieldNode.Name = CriteriaControl.Field Then

                                    ' Add to dictonary, one control one field n fieldItem
                                    For Each FieldItemNode As XmlNode In fieldNode
                                        ' Find field ID
                                        If FieldItemNode.Name = CriteriaSetting.FID Then
                                            strFieldID = FieldItemNode.InnerText
                                        End If

                                        dicFieldSetting.Add(FieldItemNode.Name, FieldItemNode.InnerText)
                                    Next

                                    ' Add to xx, one control n field n fieldItem
                                    dicBuildSetting.Add(strFieldID, dicFieldSetting)

                                End If
                            Next

                        End If
                    Next

                    ' Create control
                    Dim con As Control = LoadControl(_strControlPath + strControlName)
                    con.ID = strControlID
                    returnControlList.Add(con)
                    CType(con, IStatisticsCriteriaUC).Build(dicBuildSetting)

                End If

            Next

        End If

        Return returnControlList
    End Function

    Public Function GetRemarkResource(ByVal xmlString As String) As String
        Dim xml As New XmlDocument
        Dim strRemark As String = String.Empty

        ' String to Xml format
        xml.LoadXml(xmlString.ToString.Trim)

        ' Get controls
        Dim controlList As XmlNode
        controlList = xml.FirstChild

        If controlList.HasChildNodes Then
            Dim xmlNodeList As XmlNodeList = controlList.ChildNodes

            ' Resource remark node
            For Each listNode As XmlNode In xmlNodeList

                If listNode.Name = CriteriaControl.RemarkResource Then
                    strRemark = listNode.InnerText
                End If

            Next

        End If

        Return strRemark
    End Function

    Public Function GetResultSetup(ByVal xmlString As String) As Dictionary(Of String, Dictionary(Of String, String))
        Dim xml As New XmlDocument
        Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))

        ' String to Xml format
        xml.LoadXml(xmlString.ToString.Trim)

        ' Get results
        Dim resultList As XmlNode
        resultList = xml.FirstChild

        If resultList.HasChildNodes Then
            Dim xmlNodeList As XmlNodeList = resultList.ChildNodes

            ' Columnlist node
            For Each listNode As XmlNode In xmlNodeList
                If listNode.Name = ResultSetup.ColumnList Then

                    'Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))
                    For Each controlNode As XmlNode In listNode

                        If controlNode.Name = ResultSetup.Column Then
                            Dim fieldNodeList As XmlNodeList = controlNode.ChildNodes

                            Dim strFieldID As String = String.Empty
                            Dim dicFieldSetting As New Dictionary(Of String, String)

                            For Each fieldNode As XmlNode In fieldNodeList

                                If fieldNode.Name = ResultColumnSetting.CName Then
                                    strFieldID = fieldNode.InnerText
                                End If
                                dicFieldSetting.Add(fieldNode.Name, fieldNode.InnerText)
                            Next

                            ' Add to xx, one control n field n fieldItem
                            dicResultSetting.Add(strFieldID, dicFieldSetting)

                        End If

                    Next

                End If
            Next

        End If

        Return dicResultSetting
    End Function

    Public Function GetExportSetup(ByVal xmlString As String) As Dictionary(Of String, Dictionary(Of String, String))
        Dim xml As New XmlDocument
        Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))

        ' String to Xml format
        xml.LoadXml(xmlString.ToString.Trim)

        ' Get results
        Dim resultList As XmlNode
        resultList = xml.FirstChild

        If resultList.HasChildNodes Then
            Dim xmlNodeList As XmlNodeList = resultList.ChildNodes

            ' Columnlist node
            For Each listNode As XmlNode In xmlNodeList
                If listNode.Name = ResultSetup.ColumnList Then

                    'Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))
                    For Each controlNode As XmlNode In listNode

                        If controlNode.Name = ResultSetup.Column Then
                            Dim fieldNodeList As XmlNodeList = controlNode.ChildNodes

                            Dim strFieldID As String = String.Empty
                            Dim dicFieldSetting As New Dictionary(Of String, String)

                            For Each fieldNode As XmlNode In fieldNodeList

                                If fieldNode.Name = ResultColumnSetting.CName Then
                                    strFieldID = fieldNode.InnerText
                                End If
                                dicFieldSetting.Add(fieldNode.Name, fieldNode.InnerText)
                            Next

                            ' Add to xx, one control n field n fieldItem
                            dicResultSetting.Add(strFieldID, dicFieldSetting)

                        End If

                    Next

                End If
            Next

        End If

        Return dicResultSetting
    End Function

#Region "Db To XML"

    Public Function ConvertCriteriaSetupToXML(ByVal udtStatisticCriteriaSetupModelCollection As StatisticCriteriaSetupModelCollection) As String
        Dim strRes As String = String.Empty
        ' Control list [Start]
        strRes += StartTag(CriteriaControl.ControlList)

        ' Control level
        For Each udtStatisticCriteriaSetupModel As StatisticCriteriaSetupModel In udtStatisticCriteriaSetupModelCollection
            ' Control [Start]
            strRes += StartTag(CriteriaControl.Control)

            ' CID (Control ID)
            strRes += StartTag(CriteriaControl.CID)
            strRes += udtStatisticCriteriaSetupModel.ControlID.Trim
            strRes += EndTag(CriteriaControl.CID)

            ' ClassName
            strRes += StartTag(CriteriaControl.ClassName)
            strRes += udtStatisticCriteriaSetupModel.ControlName.Trim
            strRes += EndTag(CriteriaControl.ClassName)

            ' Setting
            strRes += StartTag(CriteriaControl.Setting)
            strRes += ConvertCriteriaSettingToXML(udtStatisticCriteriaSetupModel.StatisticCriteriaDetails)
            strRes += EndTag(CriteriaControl.Setting)

            ' DisplaySeq (Display Sequence)
            strRes += StartTag(CriteriaControl.DisplaySeq)
            strRes += udtStatisticCriteriaSetupModel.DisplaySeq.ToString.Trim
            strRes += EndTag(CriteriaControl.DisplaySeq)

            ' Control [End]
            strRes += EndTag(CriteriaControl.Control)
        Next

        ' Remark Resource
        strRes += StartTag(CriteriaControl.RemarkResource)
        strRes += EndTag(CriteriaControl.RemarkResource)

        ' Control list [End]
        strRes += EndTag(CriteriaControl.ControlList)

        Return strRes
    End Function

    Public Function ConvertCriteriaSettingToXML(ByVal udtStatisticCriteriaDetailModelCollection As StatisticCriteriaDetailModelCollection) As String
        Dim strRes As String = String.Empty

        ' Standard setting
        For Each udtStatisticCriteriaDetailModel As StatisticCriteriaDetailModel In udtStatisticCriteriaDetailModelCollection
            ' Field [Start]
            strRes += StartTag(CriteriaControl.Field)

            ' FID (Field ID)
            strRes += StartTag(CriteriaSetting.FID)
            strRes += udtStatisticCriteriaDetailModel.FieldID.Trim
            strRes += EndTag(CriteriaSetting.FID)

            ' DescResource (Description Resource)
            strRes += StartTag(CriteriaSetting.DescResource)
            strRes += udtStatisticCriteriaDetailModel.DescResource.Trim
            strRes += EndTag(CriteriaSetting.DescResource)

            ' Visible
            strRes += StartTag(CriteriaSetting.Visible)
            strRes += udtStatisticCriteriaDetailModel.Visible.Trim
            strRes += EndTag(CriteriaSetting.Visible)

            ' DefaultValue
            strRes += StartTag(CriteriaSetting.DefaultValue)
            strRes += udtStatisticCriteriaDetailModel.DefaultValue.Trim
            strRes += EndTag(CriteriaSetting.DefaultValue)

            ' SPParamName (Stored Proc Parameter Name)
            strRes += StartTag(CriteriaSetting.SPParamName)
            strRes += udtStatisticCriteriaDetailModel.SPParamName.Trim
            strRes += EndTag(CriteriaSetting.SPParamName)

            ' Addition setting 
            strRes += ConvertCriteriaAdditionSettingToXML(udtStatisticCriteriaDetailModel.StatisticCriteriaAdditionCollection)

            ' Field [End]
            strRes += EndTag(CriteriaControl.Field)
        Next

        Return strRes
    End Function

    Public Function ConvertCriteriaAdditionSettingToXML(ByVal udtStatisticCriteriaAdditionDetailModelCollection As StatisticCriteriaAdditionDetailModelCollection) As String
        Dim strRes As String = String.Empty

        If Not udtStatisticCriteriaAdditionDetailModelCollection Is Nothing Then
            For Each udtStatisticCriteriaAdditionDetailModel As StatisticCriteriaAdditionDetailModel In udtStatisticCriteriaAdditionDetailModelCollection
                strRes += StartTag(udtStatisticCriteriaAdditionDetailModel.SetupType)
                strRes += udtStatisticCriteriaAdditionDetailModel.SetupValue.Trim
                strRes += EndTag(udtStatisticCriteriaAdditionDetailModel.SetupType)
            Next
        End If

        Return strRes
    End Function

    Public Function ConvertResultSetupToXML(ByVal udtStatisticResultSetupModelCollection As StatisticResultSetupModelCollection) As String
        Dim strRes As String = String.Empty

        ' ResultList [Start]
        strRes += StartTag(ResultSetup.ResultList)

        ' ColumnList [Start]
        strRes += StartTag(ResultSetup.ColumnList)

        For Each udtStatisticResultSetupModel As StatisticResultSetupModel In udtStatisticResultSetupModelCollection
            ' Column [Start]
            strRes += StartTag(ResultSetup.Column)

            ' CName
            strRes += StartTag(ResultColumnSetting.CName)
            strRes += udtStatisticResultSetupModel.ColumnName.Trim
            strRes += EndTag(ResultColumnSetting.CName)

            ' DescResource
            strRes += StartTag(ResultColumnSetting.DescResource)
            strRes += udtStatisticResultSetupModel.DisplayDescResource.Trim
            strRes += EndTag(ResultColumnSetting.DescResource)

            ' CWidth
            strRes += StartTag(ResultColumnSetting.CWidth)
            strRes += udtStatisticResultSetupModel.DisplayColumnWidth.ToString.Trim
            strRes += EndTag(ResultColumnSetting.CWidth)

            ' ValueFormat
            strRes += StartTag(ResultColumnSetting.ValueFormat)
            strRes += udtStatisticResultSetupModel.DisplayValueFormat.Trim
            strRes += EndTag(ResultColumnSetting.ValueFormat)

            ' Column [End]
            strRes += EndTag(ResultSetup.Column)
        Next

        ' ColumnList [End]
        strRes += EndTag(ResultSetup.ColumnList)

        ' Remark Resource
        strRes += StartTag(ResultSetup.RemarkResource)
        strRes += EndTag(ResultSetup.RemarkResource)

        ' ResultList [End]
        strRes += EndTag(ResultSetup.ResultList)

        Return strRes
    End Function

    Public Function ConvertExportSetupToXML(ByVal udtStatisticResultSetupModelCollection As StatisticResultSetupModelCollection) As String
        Dim strRes As String = String.Empty

        ' ResultList [Start]
        strRes += StartTag(ExportSetup.ResultList)

        ' ColumnList [Start]
        strRes += StartTag(ExportSetup.ColumnList)

        For Each udtStatisticResultSetupModel As StatisticResultSetupModel In udtStatisticResultSetupModelCollection
            ' Column [Start]
            strRes += StartTag(ExportSetup.Column)

            ' CName
            strRes += StartTag(ExportColumnSetting.CName)
            strRes += udtStatisticResultSetupModel.ColumnName.Trim
            strRes += EndTag(ExportColumnSetting.CName)

            ' DescResource
            strRes += StartTag(ExportColumnSetting.DescResource)
            strRes += udtStatisticResultSetupModel.ExportDescResource.Trim
            strRes += EndTag(ExportColumnSetting.DescResource)

            ' CWidth
            strRes += StartTag(ExportColumnSetting.CWidth)
            strRes += udtStatisticResultSetupModel.ExportColumnWidth.ToString.Trim
            strRes += EndTag(ExportColumnSetting.CWidth)

            ' ValueFormat
            strRes += StartTag(ExportColumnSetting.ValueFormat)
            strRes += udtStatisticResultSetupModel.ExportValueFormat.Trim
            strRes += EndTag(ExportColumnSetting.ValueFormat)

            ' Column [End]
            strRes += EndTag(ExportSetup.Column)
        Next

        ' ColumnList [End]
        strRes += EndTag(ExportSetup.ColumnList)

        ' Remark Resource
        strRes += StartTag(ExportSetup.RemarkResource)
        strRes += EndTag(ExportSetup.RemarkResource)

        ' ResultList [End]
        strRes += EndTag(ExportSetup.ResultList)

        Return strRes
    End Function

#End Region

End Class
