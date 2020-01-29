Namespace BLL

    <Serializable()> Public Class EHSClaimVaccineModel

        ' EHSClaimVaccineModel: EVSS
        '   -> EHSClaimSubsidizeModel: EHSIV, EIV, EPV
        '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    1	1STDOSE             
        '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    2	2NDDOSE 

        Private _strSchemeCode As String
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Private _intSchemeSeq As Integer
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        Private _arrSubsidizeList As EHSClaimSubsidizeModelCollection

        Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Property SchemeSeq() As Integer
        '    Get
        '        Return Me._intSchemeSeq
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._intSchemeSeq = value
        '    End Set
        'End Property
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Property SubsidizeList() As EHSClaimSubsidizeModelCollection
            Get
                Return Me._arrSubsidizeList
            End Get
            Set(ByVal value As EHSClaimSubsidizeModelCollection)
                Me._arrSubsidizeList = value
            End Set
        End Property

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer)
        Public Sub New(ByVal strSchemeCode As String)
            Me._strSchemeCode = strSchemeCode
            'Me._intSchemeSeq = intSchemeSeq
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Sub Add(ByVal udtClaimSubsidize As EHSClaimSubsidizeModel)
            If Me._arrSubsidizeList Is Nothing Then Me._arrSubsidizeList = New EHSClaimSubsidizeModelCollection()
            Me._arrSubsidizeList.Add(udtClaimSubsidize)
            Me._arrSubsidizeList.Sort()
        End Sub

#Region "Class EHSClaimSubsidzeModel & Collection"

        <Serializable()> Public Class EHSClaimSubsidizeModel
            Implements IComparable
            Implements IComparer

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Add Scheme info for handle multiple scheme seq subsidy
            Private _strSchemeCode As String
            Private _intSchemeSeq As String
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            Private _strSubsidizeCode As String
            Private _intDisplaySeq As Integer
            Private _strSubsidizeItemCode As String
            Private _strSubsidizeDisplayCode As String

            Private _strSubsidizeItemDesc As String
            Private _strSubsidizeItemDescChi As String

            Private _dblAmount As Double

            Private _dblVaccineFee As Nullable(Of Double)
            Private _blnVaccineFeeEnabled As Boolean
            Private _dblInjectionFee As Nullable(Of Double)
            Private _blnInjectionFeeEnabled As Boolean

            Private _strRemark As String
            Private _strRemarkChi As String

            Private _blnSelected As Boolean

            Private _arrSubsidizeDetailModel As EHSClaimSubidizeDetailModelCollection


            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------

            Public ReadOnly Property Key() As String
                Get
                    Return Me.SchemeCode + "|" + Me.SchemeSeq.ToString() + "|" + Me.SubsidizeCode
                End Get
            End Property

            Public Property SchemeCode() As String
                Get
                    Return Me._strSchemeCode
                End Get
                Set(ByVal value As String)
                    Me._strSchemeCode = value
                End Set
            End Property

            Public Property SchemeSeq() As Integer
                Get
                    Return Me._intSchemeSeq
                End Get
                Set(ByVal value As Integer)
                    Me._intSchemeSeq = value
                End Set
            End Property
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            Property SubsidizeCode() As String
                Get
                    Return Me._strSubsidizeCode
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeCode = value
                End Set
            End Property

            Property SubsidizeDisplayCode() As String
                Get
                    Return Me._strSubsidizeDisplayCode
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeDisplayCode = value
                End Set
            End Property

            Property SubsidizeItemDesc() As String
                Get
                    Return Me._strSubsidizeItemDesc
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeItemDesc = value
                End Set
            End Property

            Property SubsidizeItemDescChi() As String
                Get
                    Return Me._strSubsidizeItemDescChi
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeItemDescChi = value
                End Set
            End Property

            Property DisplaySeq() As Integer
                Get
                    Return Me._intDisplaySeq
                End Get
                Set(ByVal value As Integer)
                    Me._intDisplaySeq = value
                End Set
            End Property

            Property Amount() As Double
                Get
                    Return Me._dblAmount
                End Get
                Set(ByVal value As Double)
                    Me._dblAmount = value
                End Set
            End Property

            Property VaccineFee() As Nullable(Of Double)
                Get
                    Return Me._dblVaccineFee
                End Get
                Set(ByVal value As Nullable(Of Double))
                    Me._dblVaccineFee = value
                End Set
            End Property

            Property VaccineFeeEnabled() As Boolean
                Get
                    Return Me._blnVaccineFeeEnabled
                End Get
                Set(ByVal value As Boolean)
                    Me._blnVaccineFeeEnabled = value
                End Set
            End Property

            Property InjectionFee() As Nullable(Of Double)
                Get
                    Return Me._dblInjectionFee
                End Get
                Set(ByVal value As Nullable(Of Double))
                    Me._dblInjectionFee = value
                End Set
            End Property

            Property InjectionFeeEnabled() As Boolean
                Get
                    Return Me._blnInjectionFeeEnabled
                End Get
                Set(ByVal value As Boolean)
                    Me._blnInjectionFeeEnabled = value
                End Set
            End Property

            Property SubsidizeItemCode() As String
                Get
                    Return Me._strSubsidizeItemCode
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeItemCode = value
                End Set
            End Property

            Property Selected() As Boolean
                Get
                    Return Me._blnSelected
                End Get
                Set(ByVal value As Boolean)
                    Me._blnSelected = value
                End Set
            End Property

            Property SubsidizeDetailList() As EHSClaimSubidizeDetailModelCollection
                Get
                    Return Me._arrSubsidizeDetailModel
                End Get
                Set(ByVal value As EHSClaimSubidizeDetailModelCollection)
                    Me._arrSubsidizeDetailModel = value
                End Set
            End Property

            Property Remark() As String
                Get
                    Return Me._strRemark
                End Get
                Set(ByVal value As String)
                    Me._strRemark = value
                End Set
            End Property

            Property RemarkChi() As String
                Get
                    Return Me._strRemarkChi
                End Get
                Set(ByVal value As String)
                    Me._strRemarkChi = value
                End Set
            End Property

            'Public Sub New(ByVal SubsidizeCode As String, ByVal intDisplaySeq As Integer, ByVal strSubsidizeItemCode As String, _
            '    ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, ByVal dblAmount As Double)
            '    Me._strSubsidizeCode = SubsidizeCode
            '    Me._intDisplaySeq = intDisplaySeq
            '    Me._strSubsidizeItemCode = strSubsidizeItemCode
            '    Me._strSubsidizeItemDesc = strSubsidizeItemDesc
            '    Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi

            'End Sub


            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Add SchemeCode and SchemeSeq as unique identifier for "Key"
            Public Sub New(ByVal SchemeCode As String, ByVal SchemeSeq As Integer, ByVal SubsidizeCode As String, _
                ByVal intDisplaySeq As Integer, ByVal strSubsidizeItemCode As String, _
                ByVal strSubsidizeDisplayCode As String, ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, _
                ByVal dblAmount As Double, ByVal dblVaccineFee As Nullable(Of Double), ByVal blnVaccineFeeEnabled As Boolean, _
                ByVal dblInjectionFee As Nullable(Of Double), ByVal blnInjectionFeeEnabled As Double)
                Me._strSchemeCode = SchemeCode
                Me._intSchemeSeq = SchemeSeq
                Me._strSubsidizeCode = SubsidizeCode
                Me._intDisplaySeq = intDisplaySeq
                Me._strSubsidizeItemCode = strSubsidizeItemCode
                Me._strSubsidizeDisplayCode = strSubsidizeDisplayCode
                Me._strSubsidizeItemDesc = strSubsidizeItemDesc
                Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi

                Me._dblAmount = dblAmount

                Me._dblVaccineFee = dblVaccineFee
                Me._blnVaccineFeeEnabled = blnVaccineFeeEnabled
                Me._dblInjectionFee = dblInjectionFee
                Me._blnInjectionFeeEnabled = blnInjectionFeeEnabled

            End Sub
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            Public Sub Add(ByVal udtClaimSubsidizeDetail As EHSClaimSubidizeDetailModel)
                If Me._arrSubsidizeDetailModel Is Nothing Then Me._arrSubsidizeDetailModel = New EHSClaimSubidizeDetailModelCollection()
                Me._arrSubsidizeDetailModel.Add(udtClaimSubsidizeDetail)
                Me._arrSubsidizeDetailModel.Sort()
            End Sub

            ReadOnly Property Available() As Boolean
                Get
                    ' If Collection Include Available Detail Item
                    Dim isAvailable As Boolean = False
                    For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me.SubsidizeDetailList
                        If udtEHSClaimSubidizeDetail.Available Then
                            isAvailable = True
                            Exit For
                        End If
                    Next

                    Return isAvailable
                End Get
            End Property

#Region "ICompares"
            Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Modify to compare with scheme seq

                Dim iResult As Integer = -1
                If obj.GetType Is GetType(EHSClaimSubsidizeModel) Then
                    iResult = Me.DisplaySeq.CompareTo(CType(obj, EHSClaimSubsidizeModel).DisplaySeq)
                    If iResult = 0 Then
                        iResult = Me.SchemeSeq.CompareTo(CType(obj, EHSClaimSubsidizeModel).SchemeSeq)
                    End If
                Else
                    iResult = -1
                End If

                Return iResult
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            End Function

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                If x.GetType Is GetType(EHSClaimSubsidizeModel) AndAlso y.GetType Is GetType(EHSClaimSubsidizeModel) Then
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    ' Rectify to use defined CompareTo function

                    Return CType(x, EHSClaimSubsidizeModel).CompareTo(CType(y, EHSClaimSubsidizeModel))
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                Else
                    If x.GetType Is GetType(EHSClaimSubsidizeModel) Then
                        Return -1
                    End If
                    If y.GetType Is GetType(EHSClaimSubsidizeModel) Then
                        Return 1
                    End If
                    Return 0
                End If
            End Function
#End Region

        End Class

        <Serializable()> Public Class EHSClaimSubsidizeModelCollection
            Inherits System.Collections.ArrayList

            Public Sub New()
            End Sub

            Public Overloads Sub Add(ByVal udtEHSClaimSubsidizeModel As EHSClaimSubsidizeModel)
                MyBase.Add(udtEHSClaimSubsidizeModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtEHSClaimSubsidizeModel As EHSClaimSubsidizeModel)
                MyBase.Remove(udtEHSClaimSubsidizeModel)
            End Sub

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Public Function Filter(ByVal udtInputSubsidizeModel As EHSClaimSubsidizeModel) As EHSClaimSubsidizeModel
                Dim udtSubsidizeResult As EHSClaimSubsidizeModel = Nothing

                For Each udtSubsidize As EHSClaimSubsidizeModel In Me
                    If udtSubsidize.Key = udtInputSubsidizeModel.Key Then
                        udtSubsidizeResult = udtSubsidize
                        Exit For
                    End If
                Next

                Return udtSubsidizeResult
            End Function
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSClaimSubsidizeModel
                Get
                    Return CType(MyBase.Item(intIndex), EHSClaimSubsidizeModel)
                End Get
            End Property
        End Class

#End Region

#Region "Class EHSClaimSubsidizeDetailModel & Collection"

        <Serializable()> Public Class EHSClaimSubidizeDetailModel
            Implements IComparable
            Implements IComparer

            Private _strSubsidizeItemCode As String
            Private _strAvailableItemCode As String
            Private _intDisplaySeq As Integer

            Private _strAvailableItemDesc As String
            Private _strAvailableItemDescChi As String

            Private _blnAvailable As Boolean
            Private _blnReceived As Boolean
            Private _blnSelected As Boolean

            Private _dtmDoseDate As Nullable(Of Date) = Nothing

#Region "Property"
            Property SubsidizeItemCode() As String
                Get
                    Return Me._strSubsidizeItemCode
                End Get
                Set(ByVal value As String)
                    Me._strSubsidizeItemCode = value
                End Set
            End Property

            Property AvailableItemCode() As String
                Get
                    Return Me._strAvailableItemCode
                End Get
                Set(ByVal value As String)
                    Me._strAvailableItemCode = value
                End Set
            End Property

            Property DisplaySeq() As Integer
                Get
                    Return Me._intDisplaySeq
                End Get
                Set(ByVal value As Integer)
                    Me._intDisplaySeq = value
                End Set
            End Property

            Property AvailableItemDesc() As String
                Get
                    Return Me._strAvailableItemDesc
                End Get
                Set(ByVal value As String)
                    Me._strAvailableItemDesc = value
                End Set
            End Property

            Property AvailableItemDescChi() As String
                Get
                    Return Me._strAvailableItemDescChi
                End Get
                Set(ByVal value As String)
                    Me._strAvailableItemDescChi = value
                End Set
            End Property

            Property Available() As Boolean
                Get
                    Return Me._blnAvailable
                End Get
                Set(ByVal value As Boolean)
                    Me._blnAvailable = value
                End Set
            End Property

            Property Received() As Boolean
                Get
                    Return Me._blnReceived
                End Get
                Set(ByVal value As Boolean)
                    Me._blnReceived = value
                End Set
            End Property

            Property Selected() As Boolean
                Get
                    Return Me._blnSelected
                End Get
                Set(ByVal value As Boolean)
                    'If (Me._blnAvailable = False OrElse Me._blnReceived) AndAlso value Then
                    '    Throw New Exception("The Vaccine is not Available, Should not be selected!")
                    'End If
                    Me._blnSelected = value
                End Set
            End Property

            Property DoseDate() As Nullable(Of Date)
                Get
                    Return Me._dtmDoseDate
                End Get
                Set(ByVal value As Nullable(Of Date))
                    Me._dtmDoseDate = value
                End Set
            End Property
#End Region

#Region "ICompares"
            Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
                If obj.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                    Return Me.DisplaySeq.CompareTo(CType(obj, EHSClaimSubidizeDetailModel).DisplaySeq)
                Else
                    Return -1
                End If
            End Function

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                If x.GetType Is GetType(EHSClaimSubidizeDetailModel) AndAlso y.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                    Return CType(x, EHSClaimSubidizeDetailModel).DisplaySeq.CompareTo(CType(y, EHSClaimSubidizeDetailModel).DisplaySeq)
                Else
                    If x.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                        Return -1
                    End If
                    If y.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                        Return 1
                    End If
                    Return 0
                End If
            End Function
#End Region

            ''' <summary>
            ''' Constructor
            ''' </summary>
            ''' <param name="strSubsidizeItemCode"></param>
            ''' <param name="strAvailableItemCode"></param>
            ''' <param name="intDisplaySeq"></param>
            ''' <param name="strAvailableItemDesc"></param>
            ''' <param name="strAvailableItemDescChi"></param>
            ''' <param name="blnAvailable"></param>
            ''' <param name="blnReceived"></param>
            ''' <param name="blnSelected"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, ByVal intDisplaySeq As Integer, _
                ByVal strAvailableItemDesc As String, ByVal strAvailableItemDescChi As String, _
                ByVal blnAvailable As Boolean, ByVal blnReceived As Boolean, ByVal blnSelected As Boolean)

                Me._strSubsidizeItemCode = strSubsidizeItemCode
                Me._strAvailableItemCode = strAvailableItemCode
                Me._intDisplaySeq = intDisplaySeq
                Me._strAvailableItemDesc = strAvailableItemDesc
                Me._strAvailableItemDescChi = strAvailableItemDescChi

                Me._blnAvailable = blnAvailable
                Me._blnReceived = blnReceived
                Me._blnSelected = blnSelected
            End Sub

        End Class

        <Serializable()> Public Class EHSClaimSubidizeDetailModelCollection
            Inherits System.Collections.ArrayList

            Public Sub New()
            End Sub

            Public Overloads Sub Add(ByVal udtEHSClaimSubidizeDetailModel As EHSClaimSubidizeDetailModel)
                MyBase.Add(udtEHSClaimSubidizeDetailModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtEHSClaimSubidizeDetailModel As EHSClaimSubidizeDetailModel)
                MyBase.Remove(udtEHSClaimSubidizeDetailModel)
            End Sub

            Public Function Filter(ByVal strAvailableItemCode As String) As EHSClaimSubidizeDetailModel
                Dim udtOutEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel = Nothing
                For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me
                    If udtEHSClaimSubidizeDetail.AvailableItemCode.Trim().Equals(strAvailableItemCode) Then
                        udtOutEHSClaimSubidizeDetail = udtEHSClaimSubidizeDetail
                        Exit For
                    End If
                Next
                Return udtOutEHSClaimSubidizeDetail
            End Function

            Public Function FilterBySubsidizeItemCode(ByVal strSubsidizeItemCode As String) As EHSClaimSubidizeDetailModel
                Dim udtOutEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel = Nothing
                For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me
                    If udtEHSClaimSubidizeDetail.SubsidizeItemCode.Trim().Equals(strSubsidizeItemCode) Then
                        udtOutEHSClaimSubidizeDetail = udtEHSClaimSubidizeDetail
                        Exit For
                    End If
                Next
                Return udtOutEHSClaimSubidizeDetail
            End Function

            Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSClaimSubidizeDetailModel
                Get
                    Return CType(MyBase.Item(intIndex), EHSClaimSubidizeDetailModel)
                End Get
            End Property

        End Class

#End Region


        '#Region "Vaccine Validation"

        '        Public Function chkVaccineSelection(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal udcMsgBoxErr As CustomControls.MessageBox) As Boolean
        '            Dim noVaccineSelected As Boolean = True
        '            Dim noAvailableVaccine As Boolean = True
        '            Dim noDoseSelected As Boolean = True
        '            Dim isValid As Boolean = True

        '            Dim systemMessages As List(Of Common.ComObject.SystemMessage) = Nothing

        '            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

        '                If Not udtEHSClaimSubsidize.Available Then

        '                    'Vaccine is not availble but selected -> Display Error Message
        '                    If udtEHSClaimSubsidize.Selected Then
        '                        udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00234"))
        '                        Return False
        '                    Else
        '                        'Dose is not availble but selected -> Display Error Message
        '                        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then

        '                            For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
        '                                If udtSubsidizeDetail.Selected Then
        '                                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
        '                                    Return False
        '                                End If
        '                            Next

        '                        End If
        '                    End If

        '                Else
        '                    noAvailableVaccine = False

        '                    If udtEHSClaimSubsidize.Selected Then
        '                        noVaccineSelected = False

        '                        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
        '                            noDoseSelected = True

        '                            '-------------------------------------------------------------------------------------------------------------------------
        '                            For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

        '                                If udtSubsidizeDetail.Selected Then

        '                                    If Not udtSubsidizeDetail.Available Then
        '                                        udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
        '                                        Return False
        '                                    Else
        '                                        noDoseSelected = False
        '                                    End If

        '                                End If
        '                            Next

        '                            If noDoseSelected Then
        '                                isValid = False
        '                                'Display message if dose is not selected 
        '                                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00190"), New String() {"%s", "%r"}, New String() {udtEHSClaimSubsidize.SubsidizeItemDesc, udtEHSClaimSubsidize.SubsidizeItemDescChi})
        '                            End If
        '                            '-------------------------------------------------------------------------------------------------------------------------
        '                        End If

        '                    End If

        '                End If
        '            Next

        '            'have available Vaccine
        '            If Not noAvailableVaccine AndAlso noVaccineSelected Then
        '                isValid = False
        '                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00189"))
        '            End If

        '            Return isValid
        '        End Function

        '        Public Function chkVaccineSelection(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal udcMsgBoxErr As CustomControls.TextOnlyMessageBox) As Boolean
        '            Dim noVaccineSelected As Boolean = True
        '            Dim noAvailableVaccine As Boolean = True
        '            Dim noDoseSelected As Boolean = True
        '            Dim isValid As Boolean = True

        '            Dim systemMessages As List(Of Common.ComObject.SystemMessage) = Nothing

        '            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

        '                If Not udtEHSClaimSubsidize.Available Then

        '                    'Vaccine is not availble but selected -> Display Error Message
        '                    If udtEHSClaimSubsidize.Selected Then
        '                        udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00234"))
        '                        Return False
        '                    Else
        '                        'Dose is not availble but selected -> Display Error Message
        '                        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then

        '                            For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
        '                                If udtSubsidizeDetail.Selected Then
        '                                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
        '                                    Return False
        '                                End If
        '                            Next

        '                        End If
        '                    End If

        '                Else
        '                    noAvailableVaccine = False

        '                    If udtEHSClaimSubsidize.Selected Then
        '                        noVaccineSelected = False

        '                        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
        '                            noDoseSelected = True

        '                            '-------------------------------------------------------------------------------------------------------------------------
        '                            For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

        '                                If udtSubsidizeDetail.Selected Then

        '                                    If Not udtSubsidizeDetail.Available Then
        '                                        udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
        '                                        Return False
        '                                    Else
        '                                        noDoseSelected = False
        '                                    End If

        '                                End If
        '                            Next

        '                            If noDoseSelected Then
        '                                isValid = False
        '                                'Display message if dose is not selected 
        '                                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00190"), New String() {"%s", "%r"}, New String() {udtEHSClaimSubsidize.SubsidizeItemDesc, udtEHSClaimSubsidize.SubsidizeItemDescChi})
        '                            End If
        '                            '-------------------------------------------------------------------------------------------------------------------------
        '                        End If

        '                    End If

        '                End If
        '            Next

        '            'have available Vaccine
        '            If Not noAvailableVaccine AndAlso noVaccineSelected Then
        '                isValid = False
        '                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00189"))
        '            End If

        '            Return isValid
        '        End Function


        '#End Region

    End Class

End Namespace


