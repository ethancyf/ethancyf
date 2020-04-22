Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionInvalidationModel

#Region "Status"

        Public Class TransactionInvalidationStatusClass
            Public Const Active As String = "A"
            Public Const Confirmed As String = "C"
            Public Const Rejected As String = "R"
            Public Const ClassCode As String = "TransactionInvalidationStatus"
        End Class

#End Region

#Region "Schema"

        ' Transaction_ID	    char	    no	20
        ' Record_Status	        char	    no	1
        ' Invalidation_Type	    char	    no	2
        ' Invalidation_Remark	varchar	    no	255
        ' Create_Dtm	        datetime	no	8
        ' Create_By	            varchar	    no	20
        ' Update_Dtm	        datetime	no	8
        ' Update_By	            varchar	    no	20
        ' TSMP	                timestamp	no	8

#End Region

#Region "SQL Data Type"

        Public Const Transaction_ID_DataType As SqlDbType = SqlDbType.Char
        Public Const Transaction_ID_DataSize As Integer = 20

#End Region

#Region "Private Member"

        Private _strTransaction_ID As String
        Private _strRecord_Status As String
        Private _strInvalidation_Type As String
        Private _strInvalidation_Remark As String
        Private _dtmCreate_Dtm As DateTime
        Private _strCreate_By As String
        Private _dtmUpdate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _byteTSMP As Byte()

#End Region

#Region "Property"

        Public Property TransactionID() As String
            Get
                Return Me._strTransaction_ID
            End Get
            Set(ByVal value As String)
                Me._strTransaction_ID = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

        Public Property InvalidationType() As String
            Get
                Return Me._strInvalidation_Type
            End Get
            Set(ByVal value As String)
                Me._strInvalidation_Type = value
            End Set
        End Property

        Public Property InvalidationRemark() As String
            Get
                Return Me._strInvalidation_Remark
            End Get
            Set(ByVal value As String)
                Me._strInvalidation_Remark = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property CreateDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCreate_Dtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property

        Public Property UpdateDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return Me._byteTSMP
            End Get
            Set(ByVal value As Byte())
                Me._byteTSMP = value
            End Set
        End Property

#End Region

#Region "Addition Member"

        ' Nothing here

#End Region

#Region "Constructor"

        Public Sub New(ByVal strTransactionID As String, ByVal strInvalidationType As String, _
                        ByVal strRecordStatus As String, ByVal strInvalidationRemark As String, ByVal dtmCreateDtm As DateTime, _
                        ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, ByVal byteTSMP As Byte())

            Me._strTransaction_ID = strTransactionID
            Me._strInvalidation_Type = strInvalidationType
            Me._strRecord_Status = strRecordStatus
            Me._strInvalidation_Remark = strInvalidationRemark
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strCreate_By = strCreateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm
            Me._strUpdate_By = strUpdateBy
            Me._byteTSMP = byteTSMP

        End Sub

#End Region

    End Class

End Namespace