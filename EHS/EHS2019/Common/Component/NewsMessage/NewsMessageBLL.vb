Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Namespace Component.NewsMessage
    Public Class NewsMessageBLL

        Public Function GetNewsMessage() As DataTable
            Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
            Dim dt As New DataTable
            Dim db As Database = New Database
            Try
                Dim prams() As SqlParameter = { _
                db.MakeInParam("@platform", SqlDbType.Char, 2, strPlatform)}
                db.RunProc("proc_NewsMessage_get", prams, dt)
            Catch ex As Exception
                'ErrorHandler.Log(ex)
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function GetNewsMessageModelCollection() As NewsMessageModelCollection
            Dim udtNewsMessageCollection As New NewsMessageModelCollection
            Dim udtNewsMessage As NewsMessageModel

            Dim dtNewsMessage As DataTable = GetNewsMessage()
            Dim drNewsMessage As DataRow
            Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            Dim dtsystemdtm As DateTime = commfunct.GetSystemDateTime

            For Each drNewsMessage In dtNewsMessage.Rows
                udtNewsMessage = New NewsMessageModel
                udtNewsMessage.MsgID = drNewsMessage.Item("Msg_ID")
                udtNewsMessage.Description = drNewsMessage.Item("Description")
                udtNewsMessage.ChiDescription = drNewsMessage.Item("Chinese_Description")
                udtNewsMessage.CNDescription = drNewsMessage.Item("CN_Description")
                udtNewsMessage.CreateDtm = drNewsMessage.Item("Create_Dtm")
                udtNewsMessage.EffectiveDtm = drNewsMessage.Item("Effective_Dtm")
                udtNewsMessage.ExpiryDtm = drNewsMessage.Item("Expiry_Dtm")
                If udtNewsMessage.EffectiveDtm <= dtsystemdtm And udtNewsMessage.ExpiryDtm >= dtsystemdtm Then
                    udtNewsMessageCollection.Add(udtNewsMessage)
                End If
            Next
            udtNewsMessageCollection.Sort()

            Return udtNewsMessageCollection
        End Function

        Private Function LoadXMLData() As DataSet
            Dim ds As DataSet = New DataSet
            Dim strFilePath As String
            strFilePath = ConfigurationManager.AppSettings("XMLPath")
            ds.ReadXml(strFilePath & "NewsMessage.xml")
            Return ds
        End Function

        Private Function GetXMLNewsMessage() As DataTable
            Dim dt As DataTable
            Dim ds As DataSet
            ds = LoadXMLData()
            dt = ds.Tables("Message")
            Return dt
        End Function

        Public Function GetNewsMessageModelCollectionFromXML() As NewsMessageModelCollection
            Dim udtNewsMessageCollection As New NewsMessageModelCollection
            Dim udtNewsMessage As NewsMessageModel

            Dim dsNewsMessage As DataSet = LoadXMLData()
            Dim dtNewsMessage As DataTable = dsNewsMessage.Tables("Message")
            Dim drNewsMessage As DataRow

            For Each drNewsMessage In dtNewsMessage.Rows
                udtNewsMessage = New NewsMessageModel
                udtNewsMessage.MsgID = drNewsMessage.Item("Msg_ID")
                udtNewsMessage.Description = drNewsMessage.Item("Description")
                udtNewsMessage.ChiDescription = drNewsMessage.Item("Chinese_Description")
                udtNewsMessage.CreateDtm = DateTime.ParseExact(drNewsMessage.Item("Create_Dtm"), "yyyy-MM-dd HH:mm:ss", Nothing)
                udtNewsMessage.EffectiveDtm = DateTime.ParseExact(drNewsMessage.Item("Effective_Dtm"), "yyyy-MM-dd HH:mm:ss", Nothing)
                udtNewsMessage.ExpiryDtm = DateTime.ParseExact(drNewsMessage.Item("Expiry_Dtm"), "yyyy-MM-dd HH:mm:ss", Nothing)

                If udtNewsMessage.EffectiveDtm <= DateTime.Now AndAlso udtNewsMessage.ExpiryDtm >= DateTime.Now Then
                    udtNewsMessageCollection.Add(udtNewsMessage)
                End If
            Next
            udtNewsMessageCollection.Sort()

            Return udtNewsMessageCollection
        End Function

    End Class
End Namespace

