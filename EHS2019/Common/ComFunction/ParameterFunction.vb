Namespace ComFunction
    <Serializable()> Public Class ParameterFunction

#Region "Constructor"
        Sub New()
        End Sub
#End Region

#Region "Inner Class: ParameterCollection"

        <Serializable()> Public Class ParameterCollection
            Inherits List(Of ParameterObject)

            Public Sub AddParam(ByVal strParamName, ByVal strValue)
                Me.Add(New ParameterObject(strParamName, strValue))
            End Sub

            Public Sub AddParam(ByVal objParamObjectWithLegend As ParameterObjectWithLegend)
                Me.Add(objParamObjectWithLegend)
            End Sub

            Public Sub AddParam(ByVal objParamObjectList As ParameterObjectList)
                Me.Add(objParamObjectList)
            End Sub
        End Class

#End Region

#Region "Inner Class: ParameterObject"

        Public Interface IParameterObject
            Property ParamName() As String
            Property ParamValue() As Object
        End Interface

        <Serializable()> Public Class ParameterObject
            Implements IParameterObject

            Private m_strParamName As String = ""
            Private m_objParamValue As String = ""

            Sub New(ByVal strName As String, ByVal strValue As String)
                Me.ParamName = strName
                Me.ParamValue = strValue
            End Sub

            Public Property ParamName() As String Implements IParameterObject.ParamName
                Get
                    Return m_strParamName
                End Get
                Set(ByVal value As String)
                    m_strParamName = value
                End Set
            End Property

            Public Property ParamValue() As Object Implements IParameterObject.ParamValue
                Get
                    Return m_objParamValue
                End Get
                Set(ByVal value As Object)
                    m_objParamValue = value
                End Set
            End Property
        End Class

        <Serializable()> Public Class ParameterObjectWithLegend
            Inherits ParameterObject

            Private m_strLegendType As String = ""

            Public Property ParamLegendType() As String
                Get
                    Return m_strLegendType
                End Get
                Set(ByVal value As String)
                    m_strLegendType = value
                End Set
            End Property

            Sub New(ByVal strName As String, ByVal strValue As String)
                MyBase.New(strName, strValue)
            End Sub
        End Class

        <Serializable()> Public Class ParameterObjectList
            Inherits ParameterObject

            Private m_listParamValue As New Collection

            Public Property ParamValueList() As Collection
                Get
                    Return m_listParamValue
                End Get
                Set(ByVal value As Collection)
                    m_listParamValue = value
                End Set
            End Property

            Sub New(ByVal strName As String)
                MyBase.New(strName, String.Empty)
            End Sub
        End Class


        '<Serializable()> Public Class ParameterObject
        '    Public ParamName As String = ""
        '    Public ParamValue As String = ""

        '    Sub New()
        '    End Sub

        '    Sub New(ByVal strName As String, ByVal strValue As String)
        '        Me.ParamName = strName
        '        Me.ParamValue = strValue
        '    End Sub
        'End Class
#End Region

#Region "Inner Class: StoreProcParamObject"
        <Serializable()> Public Class StoreProcParamObject
            Public ParamName As String = ""
            Public ParamDBType As Integer = 0
            Public ParamDBSize As String = 0
            Public ParamValue As String = ""

            Sub New()

            End Sub

            Sub New(ByVal strName As String, ByVal intDBType As Integer, ByVal intDBSize As Integer, ByVal strValue As String)
                Me.ParamName = strName
                Me.ParamDBType = intDBType
                Me.ParamDBSize = intDBSize
                Me.ParamValue = strValue
            End Sub
        End Class
#End Region

#Region "Inner Class: StoreProcParamCollection"
        <Serializable()> Public Class StoreProcParamCollection
            Inherits List(Of StoreProcParamObject)

            Public Sub AddParam(ByVal strName As String, ByVal intDBType As Integer, ByVal intDBSize As Integer, ByVal strValue As String)
                Me.Add(New StoreProcParamObject(strName, intDBType, intDBSize, strValue))
            End Sub
        End Class

#End Region

        Public Const cEnclosedStart As String = "[%"
        Public Const cEnclosedEnd As String = "%]"

        ''' <summary>
        ''' For Replace the Text With Paramter
        ''' </summary>
        ''' <param name="strParseText"></param>
        ''' <param name="udtParamList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetParsedStringByparameter(ByVal strParseText As String, ByVal udtParamList As ParameterCollection) As String
            Return Me.GetParsedStringByparameter(strParseText, udtParamList, cEnclosedStart, cEnclosedEnd)
        End Function

        ''' <summary>
        ''' For Replace the Text With Paramter
        ''' </summary>
        ''' <param name="strParseText"></param>
        ''' <param name="udtParamList"></param>
        ''' <param name="strEnclosedVariableStart"></param>
        ''' <param name="strEnclosedVariableEnd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetParsedStringByParameter(ByVal strParseText As String, ByVal udtParamList As ParameterCollection, ByVal strEnclosedVariableStart As String, ByVal strEnclosedVariableEnd As String) As String
            Dim strReturn As String = strParseText
            For Each udtParam As ParameterObject In udtParamList
                strReturn = strReturn.Replace(strEnclosedVariableStart + udtParam.ParamName + strEnclosedVariableEnd, udtParam.ParamValue)
            Next
            Return strReturn
        End Function

        Public Const cDelimitator As String = "|||"
        Public Const cPairedString As String = "==="
        Public Const cValueDelimitator As String = ";;;"

        Public Function GetParameterString(ByVal udtParamCollection As ParameterCollection, ByVal strDelimilator As String, ByVal strPairedString As String)
            Dim strParam As String = ""

            For Each udtParameter As ParameterObject In udtParamCollection
                If strParam.Trim() = "" Then
                    strParam += udtParameter.ParamName.Trim() + strPairedString + udtParameter.ParamValue.Trim()
                Else
                    strParam += strDelimilator + udtParameter.ParamName.Trim() + strPairedString + udtParameter.ParamValue.Trim()
                End If
            Next
            Return strParam
        End Function

        Public Function GetParameterString(ByVal udtParamCollection As ParameterCollection) As String
            Return Me.GetParameterString(udtParamCollection, cDelimitator, cPairedString)

        End Function

        Public Function GetParameterCollection(ByVal strParamList As String) As ParameterCollection
            Return Me.GetParameterCollection(strParamList, cDelimitator, cPairedString)
        End Function

        Public Function GetParameterCollection(ByVal strParamList As String, ByVal strDelimitator As String, ByVal strPairedString As String) As ParameterCollection
            Dim paramCollection As New ParameterCollection()
            Dim arrStrParam As String() = strParamList.Split(New String() {strDelimitator}, System.StringSplitOptions.RemoveEmptyEntries)


            For Each strParam As String In arrStrParam
                paramCollection.AddParam(strParam.Substring(0, strParam.IndexOf(strPairedString)), strParam.Substring(strParam.IndexOf(strPairedString) + strPairedString.Length))
            Next
            Return paramCollection
        End Function


        Public Function GetSPParamString(ByVal udtSPParamCollection As StoreProcParamCollection) As String
            ' To Do: Enhance to Handle DateTime Format if needed
            Dim strParam As String = ""
            For Each udtSPParam As StoreProcParamObject In udtSPParamCollection
                'CRE13-003 Token Replacement [Start][Karl]
                If udtSPParam.ParamValue Is Nothing Then udtSPParam.ParamValue = ""
                'CRE13-003 Token Replacement [End][Karl]

                If strParam.Trim() = "" Then
                    strParam += udtSPParam.ParamName + cPairedString + udtSPParam.ParamDBType.ToString() + cValueDelimitator + udtSPParam.ParamDBSize.ToString() + cValueDelimitator + udtSPParam.ParamValue.ToString()
                Else
                    strParam += cDelimitator + udtSPParam.ParamName + cPairedString + udtSPParam.ParamDBType.ToString() + cValueDelimitator + udtSPParam.ParamDBSize.ToString() + cValueDelimitator + udtSPParam.ParamValue.ToString()
                End If
            Next

            Return strParam
        End Function

        Public Function GetSPParamCollection(ByVal strParamList As String) As StoreProcParamCollection

            Dim spParamCollection = New StoreProcParamCollection()
            Dim arrStrParam As String() = strParamList.Split(New String() {cDelimitator}, System.StringSplitOptions.RemoveEmptyEntries)

            For Each strParam As String In arrStrParam
                Dim strName As String = strParam.Substring(0, strParam.IndexOf(cPairedString))
                Dim strPaired As String = strParam.Substring(strParam.IndexOf(cPairedString) + cPairedString.Length())


                Dim arrStrValue As String() = strPaired.Split(New String() {cValueDelimitator}, System.StringSplitOptions.RemoveEmptyEntries)

                If arrStrValue.Length = 3 Then
                    spParamCollection.AddParam(strName, arrStrValue(0), arrStrValue(1), arrStrValue(2))
                ElseIf arrStrValue.Length = 2 Then
                    spParamCollection.AddParam(strName, arrStrValue(0), arrStrValue(1), String.Empty)
                Else
                    Throw New ArgumentException("Fail to Parse the ParamCollection")
                End If

            Next
            Return spParamCollection
        End Function
    End Class



End Namespace