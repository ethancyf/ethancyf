Imports System.Configuration
Imports System.Xml
''' <summary>
''' AppConfigFileSettings: This class is used to Change the 
''' AppConfigs Parameters at runtime through User Interface
''' </summary>
''' <remarks></remarks>
Public Class AppConfigFileSettings
    ''' <summary>
    ''' UpdateAppSettings: It will update the app.Config file AppConfig key values
    ''' </summary>
    ''' <param name="KeyName">AppConfigs KeyName</param>
    ''' <param name="KeyValue">AppConfigs KeyValue</param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateAppSettings(ByVal KeyName As String, ByVal KeyValue As String)
        '  AppDomain.CurrentDomain.SetupInformation.ConfigurationFile 
        ' This will get the app.config file path from Current application Domain
        Dim XmlDoc As New XmlDocument()
        Dim strFileName As String = "Setting.config"
        ' Load XML Document
        XmlDoc.Load(IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, strFileName))
        ' Navigate Each XML Element of app.Config file
        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "appSettings" Then
                ' Loop each node of appSettings Element 
                ' xNode.Attributes(0).Value , Mean First Attributes of Node , 
                ' KeyName Portion
                ' xNode.Attributes(1).Value , Mean Second Attributes of Node,
                ' KeyValue Portion
                For Each xNode As XmlNode In xElement.ChildNodes
                    If Not TypeOf xNode Is XmlElement Then Continue For

                    If xNode.Attributes(0).Value = KeyName Then
                        xNode.Attributes(1).Value = KeyValue
                    End If
                Next
            End If
        Next
        ' Save app.config file
        XmlDoc.Save(strFileName)
    End Sub

    Public Shared Function GetAppSettings(ByVal KeyName As String) As String
        '  AppDomain.CurrentDomain.SetupInformation.ConfigurationFile 
        ' This will get the app.config file path from Current application Domain
        Dim XmlDoc As New XmlDocument()
        Dim strFileName As String = "Setting.config"
        ' Load XML Document
        XmlDoc.Load(strFileName)
        ' Navigate Each XML Element of app.Config file
        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "appSettings" Then
                ' Loop each node of appSettings Element 
                ' xNode.Attributes(0).Value , Mean First Attributes of Node , 
                ' KeyName Portion
                ' xNode.Attributes(1).Value , Mean Second Attributes of Node,
                ' KeyValue Portion
                For Each xNode As XmlNode In xElement.ChildNodes
                    If Not TypeOf xNode Is XmlElement Then Continue For

                    If xNode.Attributes(0).Value = KeyName Then
                        Return xNode.Attributes(1).Value
                    End If
                Next
            End If
        Next

        Return String.Empty
    End Function
End Class