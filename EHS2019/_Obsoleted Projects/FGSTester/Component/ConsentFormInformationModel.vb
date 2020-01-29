Public Class ConsentFormInformationModel

#Region "Class"

    Public Class FormTypeClass
        Public Const HCVS As String = "HCVS"
        Public Const CIVSS As String = "CIVSS"
        Public Const EVSS As String = "EVSS"
    End Class

    Public Class LanguageClass
        Public Const Chinese As String = "Chinese"
    End Class

    Public Class DocTypeClass
        Public Const HKIC As String = "HKIC"
        Public Const EC As String = "EC"
        Public Const HKBC As String = "HKBC"
        Public Const REPMT As String = "REPMT"
        Public Const DocI As String = "Doc/I"
        Public Const ID235B As String = "ID235B"
        Public Const VISA As String = "VISA"
        Public Const ADOPC As String = "ADOPC"
    End Class

    Public Class FormStyleClass
        Public Const Full As String = "Full"
        Public Const Condensed As String = "Condensed"
    End Class

    Public Class ReadSmartIDClass
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
        Public Const Unknown As String = ""
    End Class

    Public Class CIVSSSubsidyInfoClass
        Public Const Dose1 As String = "1STDOSE"
        Public Const Dose2 As String = "2NDDOSE"
        Public Const DoseOnly As String = "ONLYDOSE"
    End Class

    Public Class PreschoolClass
        Public Const Not1stDose As String = "Not1stDose"
        Public Const Preschool As String = "Preschool"
        Public Const NonPreschool As String = "NonPreschool"
        Public Const Unknown As String = "Unknown"
    End Class

    Public Class EVSSSubsidyInfoClass
        Public Const PV As String = "PV"
        Public Const IV As String = "IV"
    End Class

#End Region

End Class
