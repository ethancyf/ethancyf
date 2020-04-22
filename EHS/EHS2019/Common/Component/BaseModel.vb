<Serializable()> _
Public Class BaseModel

#Region "Support Function"
    Protected Function ReadDataRowString(ByVal dr As DataRow, ByVal strFieldName As String) As String
        If dr(strFieldName) Is DBNull.Value Then Return String.Empty
        Return Trim(dr(strFieldName))
    End Function

    Protected Function ReadDataRowInteger(ByVal dr As DataRow, ByVal strFieldName As String) As Nullable(Of Integer)
        If dr(strFieldName) Is DBNull.Value Then Return Nothing
        Return CInt(dr(strFieldName))
    End Function

    Protected Function ReadDataRowDouble(ByVal dr As DataRow, ByVal strFieldName As String) As Nullable(Of Decimal)
        If dr(strFieldName) Is DBNull.Value Then Return Nothing
        Return CDbl(dr(strFieldName))
    End Function

    Protected Function ReadDataRowDatetime(ByVal dr As DataRow, ByVal strFieldName As String) As Nullable(Of DateTime)
        If dr(strFieldName) Is DBNull.Value Then Return Nothing
        Return dr(strFieldName)
    End Function

    Protected Function ReadDataRowTSMP(ByVal dr As DataRow, ByVal strFieldName As String) As Byte()
        If dr(strFieldName) Is DBNull.Value Then Return Nothing
        Return dr(strFieldName)
    End Function

    ''' <summary>
    ''' Convert datarow value to enum by enum item name
    ''' </summary>
    ''' <typeparam name="T">Dynamic enum type</typeparam>
    ''' <param name="dr">Datarow</param>
    ''' <param name="strFieldName">Field name in datarow</param>
    ''' <returns>Dynamic enum value</returns>
    ''' <remarks></remarks>
    Protected Function ReadDataRowEnumName(Of T)(ByVal dr As DataRow, ByVal strFieldName As String) As T
        Return ReadDataRowEnumName(Of T)(dr(strFieldName))
    End Function

    Protected Function ReadDataRowEnumName(Of T)(ByVal strValue As String) As T
        Return DirectCast([Enum].Parse(GetType(T), strValue), T)
    End Function

    ''' <summary>
    ''' Convert datarow value to enum by enum item value
    ''' </summary>
    ''' <typeparam name="T">Dynamic enum type</typeparam>
    ''' <param name="dr">Datarow</param>
    ''' <param name="strFieldName">Field name in datarow</param>
    ''' <returns>Dynamic enum value</returns>
    ''' <remarks></remarks>
    Protected Function ReadDataRowEnumValue(Of T)(ByVal dr As DataRow, ByVal strFieldName As String) As T
        Return DirectCast(Asc(Trim(dr(strFieldName))), Object)
    End Function
#End Region
End Class

