Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComObject
Imports Common.Format
Imports Common.Component
Imports Common.Component.DHCClaim
Imports Common.Component.DHCClaim.DHCClaimBLL


Namespace Component.DHCClaim

    Public Class DHCClaimBLL
        <Serializable()> Public Class DHCPersonalInformationModel

#Region "Constructor"
 



            Public Sub New()

            End Sub

            Private Sub New(ByVal udtEHSPersonalInformationModel As DHCPersonalInformationModel)

                Me._strProf_Code = udtEHSPersonalInformationModel._strProf_Code
                Me._dtm_DOB = udtEHSPersonalInformationModel._dtm_DOB
            End Sub

            Private Sub New(ByVal strId As String, ByVal strdob As String)

                Me._strProf_Code = strId
                Me._dtm_DOB = strdob
            End Sub

            Sub New(strSP_ID As String, strProf_Code As String, strProf_RegNo As String, _
                            str_HKID As String, str_Doc_Type As String, str_HKIC_Symbol As String, _
                            str_DOB_Format As String, dtm_DOB As String, str_DHCDistrictCode As String, dbl_ClaimAmount As Double)

                Me._strSP_ID = strSP_ID
                Me._strProf_Code = strProf_Code
                Me._strProf_RegNo = strProf_RegNo
                Me._str_HKID = str_HKID
                Me._str_Doc_Type = str_Doc_Type
                Me._str_HKIC_Symbol = str_HKIC_Symbol
                Me._str_DOB_Format = str_DOB_Format
                Me._dtm_DOB = dtm_DOB
                Me._str_DHCDistrictCode = str_DHCDistrictCode
                Me._dbl_ClaimAmount = dbl_ClaimAmount

                Constructor(strSP_ID, strProf_Code, strProf_RegNo, str_HKID, str_Doc_Type, str_HKIC_Symbol, str_DOB_Format, dtm_DOB, str_DHCDistrictCode, dbl_ClaimAmount)
            End Sub

            

            Public Sub Constructor(ByVal strSP_ID As String, ByVal strProf_Code As String, ByVal strProf_RegNo As String, _
                           ByVal str_HKID As String, ByVal str_Doc_Type As String, ByVal str_HKIC_Symbol As String, _
                           ByVal str_DOB_Format As String, ByVal dtm_DOB As String, ByVal str_DHCDistrictCode As String, ByVal dbl_ClaimAmount As Double)

                Me._strSP_ID = strSP_ID
                Me._strProf_Code = strProf_Code
                Me._strProf_RegNo = strProf_RegNo
                Me._str_HKID = str_HKID
                Me._str_Doc_Type = str_Doc_Type
                Me._str_HKIC_Symbol = str_HKIC_Symbol
                Me._str_DOB_Format = str_DOB_Format
                Me._dtm_DOB = dtm_DOB
                Me._str_DHCDistrictCode = str_DHCDistrictCode
                Me._dbl_ClaimAmount = dbl_ClaimAmount

            End Sub
#End Region


#Region "Memeber"
            Private _strSP_ID As String
            Private _strProf_Code As String
            Private _strProf_RegNo As String
            Private _str_HKID As String
            Private _str_Doc_Type As String
            Private _str_HKIC_Symbol As String
            Private _str_DOB_Format As String
            Private _dtm_DOB As String
            Private _str_DHCDistrictCode As String
            Private _dbl_ClaimAmount As Double
#End Region


#Region "Property"


            Public Property DHCDistrictCode() As String
                Get
                    Return Me._str_DHCDistrictCode
                End Get
                Set(ByVal value As String)
                    Me._str_DHCDistrictCode = value
                End Set
            End Property

            Public Property SPID() As String
                Get
                    Return Me._strSP_ID
                End Get
                Set(ByVal value As String)
                    Me._strSP_ID = value
                End Set
            End Property

            Public Property ProfCode() As String
                Get
                    Return Me._strProf_Code
                End Get
                Set(ByVal value As String)
                    Me._strProf_Code = value
                End Set
            End Property

            Public Property Prof_RegNo() As String
                Get
                    Return Me._strProf_RegNo
                End Get
                Set(ByVal value As String)
                    Me._strProf_RegNo = value
                End Set
            End Property

            Public Property HKID() As String
                Get
                    Return Me._str_HKID
                End Get
                Set(ByVal value As String)
                    Me._str_HKID = value
                End Set
            End Property

            Public Property Doc_Type() As String
                Get
                    Return Me._str_Doc_Type
                End Get
                Set(ByVal value As String)
                    Me._str_Doc_Type = value
                End Set
            End Property

            Public Property DOB_Format() As String
                Get
                    Return Me._str_DOB_Format
                End Get
                Set(ByVal value As String)
                    Me._str_DOB_Format = value
                End Set
            End Property
            Public Property HKIC_Symbol() As String
                Get
                    Return Me._str_HKIC_Symbol
                End Get
                Set(ByVal value As String)
                    Me._str_HKIC_Symbol = value
                End Set
            End Property
            Public Property DOB() As String
                Get
                    Return Me._dtm_DOB
                End Get
                Set(ByVal value As String)
                    Me._dtm_DOB = value.Trim()
                End Set
            End Property
            Public Property Claim_Amount() As Double
                Get
                    Return Me._dbl_ClaimAmount
                End Get
                Set(ByVal value As Double)
                    Me._dbl_ClaimAmount = value
                End Set
            End Property
#End Region

             

        End Class
    End Class
End Namespace
