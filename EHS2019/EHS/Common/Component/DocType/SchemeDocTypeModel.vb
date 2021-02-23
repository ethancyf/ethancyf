Imports System.Data.SqlClient

Namespace Component.DocType
    <Serializable()> Public Class SchemeDocTypeModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Doc_Code	char(20)	
        'Major_Doc	char(1)
#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _strDoc_Code As String
        Private _strMajor_Doc As String

        Private _intAge_LowerLimit As Nullable(Of Integer)
        Private _strAge_LowerLimitUnit As String
        Private _intAge_UpperLimit As Nullable(Of Integer)
        Private _strAge_UpperLimitUnit As String
        Private _strAge_CalMethod As String
#End Region

#Region "SQL Data Type"

        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const Doc_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Doc_Code_DataSize As Integer = 20

        Public Const Major_Doc_DataType As SqlDbType = SqlDbType.Char
        Public Const Major_Doc_DataSize As Integer = 1

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property DocCode() As String
            Get
                Return Me._strDoc_Code
            End Get
            Set(ByVal value As String)
                Me._strDoc_Code = value
            End Set
        End Property

        Public Property IsMajorDoc() As Boolean
            Get
                If Me._strMajor_Doc.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strMajor_Doc = strYES
                Else
                    Me._strMajor_Doc = strNO
                End If
            End Set
        End Property

        Public Property AgeLowerLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_LowerLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_LowerLimit = value
            End Set
        End Property

        Public Property AgeLowerLimitUnit() As String
            Get
                Return Me._strAge_LowerLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_LowerLimitUnit = value
            End Set
        End Property

        Public Property AgeUpperLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_UpperLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_UpperLimit = value
            End Set
        End Property

        Public Property AgeUpperLimitUnit() As String
            Get
                Return Me._strAge_UpperLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_UpperLimitUnit = value
            End Set
        End Property

        Public Property AgeCalMethod() As String
            Get
                Return Me._strAge_CalMethod
            End Get
            Set(ByVal value As String)
                Me._strAge_CalMethod = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeDocTypeModel As SchemeDocTypeModel)

            Me._strScheme_Code = udtSchemeDocTypeModel._strScheme_Code
            Me._strDoc_Code = udtSchemeDocTypeModel._strDoc_Code
            Me._strMajor_Doc = udtSchemeDocTypeModel._strMajor_Doc

            Me._intAge_LowerLimit = udtSchemeDocTypeModel._intAge_LowerLimit
            Me._strAge_LowerLimitUnit = udtSchemeDocTypeModel._strAge_LowerLimitUnit
            Me._intAge_UpperLimit = udtSchemeDocTypeModel._intAge_UpperLimit
            Me._strAge_UpperLimitUnit = udtSchemeDocTypeModel._strAge_UpperLimitUnit
            Me._strAge_CalMethod = udtSchemeDocTypeModel._strAge_CalMethod

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal strMajorDoc As String, _
            ByVal intAgeLowerLimit As Nullable(Of Integer), ByVal strAgeLowerLimitUnit As String, ByVal intAgeUpperLimit As Nullable(Of Integer), _
            ByVal strAgeUpperLimitUnit As String, ByVal strAgeCalMethod As String)

            Me._intAge_LowerLimit = intAgeLowerLimit
            Me._strAge_LowerLimitUnit = strAgeLowerLimitUnit
            Me._intAge_UpperLimit = intAgeUpperLimit
            Me._strAge_UpperLimitUnit = strAgeUpperLimitUnit
            Me._strAge_CalMethod = strAgeCalMethod

            Me._strScheme_Code = strSchemeCode
            Me._strDoc_Code = strDocCode
            Me._strMajor_Doc = strMajorDoc
        End Sub

#End Region

#Region "Functions"

        Public Function IsExceedAgeLimit(dtmDOB As Date, dtmServiceReceiveDtm As Date) As Boolean
            If Me.AgeLowerLimit.HasValue Then
                If ConvertPassValueByCalUnit(Me.AgeLowerLimitUnit, dtmDOB, dtmServiceReceiveDtm) < Me.AgeLowerLimit.Value Then
                    Return True
                End If

            End If

            If Me.AgeUpperLimit.HasValue Then
                If ConvertPassValueByCalUnit(Me.AgeUpperLimitUnit, dtmDOB, dtmServiceReceiveDtm) >= Me.AgeUpperLimit.Value Then
                    Return True
                End If
            End If

            Return False

        End Function

        Private Shared Function ConvertPassValueByCalUnit(strUnit As String, dtmPassDOB As DateTime, dtmCompareDate As Date) As Integer
            '   Y   = Year (exact Year)
            '   YC  = Year (Calendar Year)
            '   M   = Month (exact Month)
            '   MC  = Month (Calendar Month)
            '   D   = Day (exact Day)
            '   W   = Week (exact Week)
            Dim intReferenceValue As Integer = -1

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    Dim intCompareYear As Integer = dtmCompareDate.Year
                    Dim intPassYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCompareYear - intPassYear

                    If (dtmPassDOB.Month > dtmCompareDate.Month) OrElse (dtmPassDOB.Month = dtmCompareDate.Month AndAlso dtmPassDOB.Day > dtmCompareDate.Day) Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "YC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCurYear - intDOBYear

                Case "M"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month

                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)
                    If dtmPassDOB.Day > dtmCompareDate.Day Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "MC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month
                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)

                Case "D"
                    intReferenceValue = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)

                Case "W"
                    Dim intDifferentDay As Integer = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)
                    intReferenceValue = CInt(Math.Floor(intDifferentDay / 7))
            End Select

            Return intReferenceValue

        End Function

#End Region

    End Class
End Namespace