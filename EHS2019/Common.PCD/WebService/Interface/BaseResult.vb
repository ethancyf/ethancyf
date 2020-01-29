Imports Common.Component.Mapping
Imports Common.ComObject


Namespace WebService.Interface

    <Serializable()> Public Class BaseResult

        Protected _objSysMsg As SystemMessage = Nothing

        Public ReadOnly Property SystemMessage() As SystemMessage
            Get
                Return _objSysMsg
            End Get
        End Property

        Protected Function GetMapping(ByVal eCodeType As EnumConstant.EnumMappingCodeType, ByVal strSourceCode As String) As CodeMappingModel
            Dim udtCodeMapList As CodeMappingCollection
            Dim udtCodeMap As CodeMappingModel

            udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
            udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, eCodeType.ToString, strSourceCode)

            Return udtCodeMap
        End Function

        Protected Overridable Sub GenReturnCodeSystemMessage(ByVal eCodeType As EnumConstant.EnumMappingCodeType, ByVal strReturnCode As String)
            Dim strreturnCodeDesc As String = String.Empty
            Dim FUNCTION_CODE_HCVU As String = "990001"
            Dim FUNCTION_CODE_HCSP As String = "990002"
            Dim SEVERITY_CODE As String = "E"

            ' Function Code depends on platform
            Dim strFunctCode As String = System.Configuration.ConfigurationManager.AppSettings("Platform")
            strFunctCode = IIf(strFunctCode = Common.Component.EVSPlatform.HCVU, FUNCTION_CODE_HCVU, IIf(strFunctCode = Common.Component.EVSPlatform.HCSP, FUNCTION_CODE_HCSP, strFunctCode))

            Dim udtCodeMap As CodeMappingModel = ComFunction.GetMappingForEHS(eCodeType, strReturnCode)
            _objSysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
        End Sub
    End Class

End Namespace
