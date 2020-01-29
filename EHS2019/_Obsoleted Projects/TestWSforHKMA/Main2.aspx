<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main2.aspx.vb" validateRequest="false" Inherits="TestWSforHKMA._Default1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Test Web Services for HKMA</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:MultiView ID="mveTest" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewInput" runat="server">
            
                <asp:Label ID="Label20" runat="server" Text="WS functions" Font-Bold="true" /> 
                <div>
                <asp:RadioButton ID="rboUploadClaimHL7" runat="server" Checked="True" GroupName="WS_Selection" Text="Upload Claim"  />
                <asp:RadioButton ID="rboUploadClaim" runat="server" Checked="False" GroupName="WS_Selection" Text="Upload Claim (For Debug Use)" AutoPostBack="true" Visible="false"  />
                <asp:RadioButton ID="rboRCHNameQuery" runat="server" GroupName="WS_Selection" Text="RCH Name Query"  />
                <asp:RadioButton ID="rboGetReasonForVisit" runat="server" GroupName="WS_Selection" Text="Get Reason For Visit"  />
                <asp:RadioButton ID="rboEHSValidatedAccountQuery" runat="server" GroupName="WS_Selection" Text="eHS Validated Account Query"  />
                <asp:RadioButton ID="rboEHSAccountSubsidyQuery" runat="server" GroupName="WS_Selection"  Text="eHS Account Voucher Query"  />
                <asp:RadioButton ID="rboSPPracticeValidation" runat="server" GroupName="WS_Selection" Text="SP Practice Validation"  />
                <asp:Button ID="btnFreeText" runat="server" Text="Free Text XML" Visible="false" />
                </div>
                <br />
                 
                <asp:CheckBox ID="chkSPInfo" runat="server" Text="Service Provider" Font-Bold="true"   />    
                <asp:Panel ID="pnlSPInfo" runat="server" >
                      <table>
                            <tr>
                                <td style="width:150px"><asp:CheckBox ID="chkSPID" runat="server" Text="SP ID" Width="150px" ></asp:CheckBox></td>
                                <td style="width:200px"><asp:TextBox ID="txtSPID" runat="server" ></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:CheckBox ID="chkSPSurname" runat="server" Text="SP Surname" ></asp:CheckBox><asp:CheckBox ID="chkSPGivenname" runat="server" Text="SP Given Name" ></asp:CheckBox></td>
                                <td><asp:TextBox ID="txtSPSurname" runat="server" ></asp:TextBox><asp:TextBox ID="txtSPGivenName" runat="server" ></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="height: 26px"><asp:CheckBox ID="chkPracticeID" runat="server" Text="Practice ID" ></asp:CheckBox></td>
                                <td style="height: 26px"><asp:TextBox ID="txtPracticeID" runat="server" ></asp:TextBox></td>
                            </tr>   
                            <tr>
                                <td><asp:CheckBox ID="chkPracticeName" runat="server" Text="Practice Name" ></asp:CheckBox></td>
                                <td><asp:TextBox ID="txtPracticeName" runat="server" ></asp:TextBox></td>
                            </tr>                              
                      </table>
                 </asp:Panel>
                <br />
     
                <asp:CheckBox ID="chkAccountInfo" runat="server" Text="Account Info" Font-Bold="true"   />   

                <asp:Panel ID="pnlAccountInfo" runat="server"  >
                  <div style="width:600">
                    <asp:RadioButton ID="rboHKIC" runat="server" Checked="True" GroupName="DocType" Text="HKIC" />
                    <asp:RadioButton ID="rboEC" runat="server" GroupName="DocType" Text="EC" />
                    <asp:RadioButton ID="rboHKBC" runat="server" GroupName="DocType" Text="HKBC" />
                    <asp:RadioButton ID="rboADOPC" runat="server" GroupName="DocType" Text="ADOPC" />
                    <asp:RadioButton ID="rboVISA" runat="server" GroupName="DocType"  Text="VISA" />
                    <asp:RadioButton ID="rboID235B" runat="server" GroupName="DocType" Text="ID235B" />
                    <asp:RadioButton ID="rboDOCI" runat="server" GroupName="DocType"  Text="DOCI" />
                    <asp:RadioButton ID="rboREPMT" runat="server" GroupName="DocType" Text="REMPT" />
                </div>
                  <table>
                        <tr>
                            <td style="width:150px"><asp:CheckBox ID="chkEntryNo" runat="server" Text="Entry No" Width="150px"   /></td>
                            <td style="width:200px"><asp:TextBox ID="txtEntryNo" runat="server"  ></asp:TextBox></td>
                            <td style="width:150px"><asp:CheckBox ID="chkDocumentNo" runat="server" Text="Document No" Width="150px"   ></asp:CheckBox></td>
                            <td style="width:200px"><asp:TextBox ID="txtDocumentNo" runat="server" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkHKIC" runat="server" Text="HKIC" ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtHKIC" runat="server" ></asp:TextBox></td>
                             <td><asp:CheckBox ID="chkRegNo" runat="server" Text="Registration No"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtRegNo" runat="server"  ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkBirthEntryNo" runat="server" Text="Birth Entry No"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtBirthEntryNo" runat="server"  ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkPermitNo" runat="server" Text="Permit No"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtPermitNo" runat="server" ></asp:TextBox></td>
                        </tr>  
                        <tr>
                            <td><asp:CheckBox ID="chkVISANo" runat="server" Text="VISA No" ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtVISANo" runat="server"  ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkSurname" runat="server" Text="Surname" ></asp:CheckBox><asp:CheckBox ID="chkGivenName" runat="server" Text="Given Name"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtSurname" runat="server"  ></asp:TextBox><asp:TextBox ID="txtGivenName" runat="server"  ></asp:TextBox></td>
                        </tr>        
                        <tr>
                            <td><asp:CheckBox ID="chkGender" runat="server" Text="Gender"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtGender" runat="server"  ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkDOB" runat="server" Text="Date of Birth"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtDOB" runat="server"  ></asp:TextBox></td>
                        </tr>   
                        <tr>
                            <td><asp:CheckBox ID="chkDOBType" runat="server" Text="(DOB Type)" ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtDOBType" runat="server"  ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkAgeOn" runat="server" Text="Age On"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtAgeOn" runat="server" ></asp:TextBox></td>
                        </tr>   
                        <tr>
                            <td><asp:CheckBox ID="chkDOReg" runat="server" Text="Date of Registration" ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtDOReg" runat="server" ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkDOBInWord" runat="server" Text="DOB In Word"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtDOBInWord" runat="server"  ></asp:TextBox></td>
                        </tr>  
                        <tr>
                            <td><asp:CheckBox ID="chkNameChi" runat="server" Text="Chinese Name"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtNameChi" runat="server" ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkDOI" runat="server" Text="Date of Issue"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtDOI" runat="server" ></asp:TextBox></td>
                        </tr> 
                        <tr>
                            <td><asp:CheckBox ID="chkSerialNo" runat="server" Text="Serial No"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtSerialNo" runat="server" Enabled="false" ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkReference" runat="server" Text="Reference"></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtReference" runat="server" Enabled="false" ></asp:TextBox></td>
                        </tr> 
                        <tr>
                            <td><asp:CheckBox ID="chkFreeReference" runat="server" Text="Free Reference"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtFreeReference" runat="server" ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkRemainUntil" runat="server" Text="Remain Until"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtRemainUntil" runat="server"  ></asp:TextBox></td>
                        </tr>   
                        <tr>
                            <td><asp:CheckBox ID="chkPassportNo" runat="server" Text="Passport No"  ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtPassportNo" runat="server"  ></asp:TextBox></td>
                            <td><asp:CheckBox ID="chkRCHCode" runat="server" Text="RCH Code" ></asp:CheckBox></td>
                            <td><asp:TextBox ID="txtRCHCode" runat="server"  ></asp:TextBox></td>
                        </tr>                           
                  </table>
             </asp:Panel>  
            <br />
            
            <asp:CheckBox ID="chkClaimInfo" runat="server" Text="Claim" Font-Bold="true"   />           
   
            <asp:Panel ID="pnlClaim" runat="server">
                <table>
                    <tr>
                        <td style="width:150px"><asp:CheckBox ID="chkServiceDate" runat="server" Text="Service Date" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:150px"><asp:TextBox ID="txtServiceDate" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>
                </table>
                <br />
                
                 <table style="background-color:White">
                    <tr>
                        <td><asp:CheckBox ID="chkHCVS" runat="server" Text="Voucher" Width="150px" Enabled="true" /></td>
                        <td style="width:200px"><asp:CheckBox ID="chkVoucherClaimed" runat="server" Text="Voucher Claimed" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtVoucherClaimed" runat="server" Enabled="true"></asp:TextBox></td>
                        <td style="width:200px"><asp:CheckBox ID="chkProfCode" runat="server" Text="Professional Code" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtProfCode" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="width:200px"><asp:CheckBox ID="chkL1Code" runat="server" Text="L1 Code" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtL1Code" runat="server" Enabled="true"></asp:TextBox></td>
                        <td style="width:200px"><asp:CheckBox ID="chkL1DescEng" runat="server" Text="L1 Desc Eng" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtL1DescEng" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkL2Code" runat="server" Text="L2 Code" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td><asp:TextBox ID="txtL2Code" runat="server" Enabled="true"></asp:TextBox></td>
                        <td><asp:CheckBox ID="chkL2DescEng" runat="server" Text="L2 Desc Eng" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td><asp:TextBox ID="txtL2DescEng" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>                    
                </table>
                
                
                <asp:Panel ID="pnlIndicator" runat="server" >    
                     <table>
                        <tr>
                            <td><asp:CheckBox ID="chkPreSchoolInd" runat="server" Text="PreSchoolInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtPreSchoolInd" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkDoseIntervalInd" runat="server" Text="DoseIntervalInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtDoseIntervalInd" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkTSWInd" runat="server" Text="TSWInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtTSWInd" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>                    
                    </table>
                </asp:Panel>
                <br />
                
                <table style="background-color:White">
                    <tr>
                        <td><asp:Label ID="lblSchemeCode" runat="server" Text="+ Scheme Code" Width="150px" /></td>
                        <td style="width:200px"><asp:TextBox ID="txtSchemeCode" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        <td></td>       
                        <td></td>
                        <td></td>           
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkVaccine1" runat="server" Text="Vaccine 1" Width="150px" Enabled="true" /></td>
                        <td style="width:200px"><asp:CheckBox ID="chkSubsidyCode1" runat="server" Text="Subsidy Code" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtSubsidyCode1" runat="server" Enabled="true"></asp:TextBox></td>
                        <td style="width:200px"><asp:CheckBox ID="chkDoseSeq1" runat="server" Text="Dose Seq" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtDoseSeq1" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="chkVaccine2" runat="server" Text="Vaccine 2" Width="150px" Enabled="true" /></td>
                        <td style="width:200px"><asp:CheckBox ID="chkSubsidyCode2" runat="server" Text="Subsidy Code" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtSubsidyCode2" runat="server" Enabled="true"></asp:TextBox></td>
                        <td style="width:200px"><asp:CheckBox ID="chkDoseSeq2" runat="server" Text="Dose Seq" Width="150px" Enabled="true"></asp:CheckBox></td>
                        <td style="width:200px"><asp:TextBox ID="txtDoseSeq2" runat="server" Enabled="true"></asp:TextBox></td>
                    </tr>                   
                </table>
                
                 <asp:Panel ID="Panel1" runat="server" >
                     <table>
                        <tr>
                            <td><asp:CheckBox ID="chkPreSchoolInd_vaccine" runat="server" Text="PreSchoolInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtPreSchoolInd_vaccine" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkDoseIntervalInd_vaccine" runat="server" Text="DoseIntervalInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtDoseIntervalInd_vaccine" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkTSWInd_vaccine" runat="server" Text="TSWInd" Width="150px" /></td>
                            <td style="width:200px"><asp:TextBox ID="txtTSWInd_vaccine" runat="server"  Width="150px" Enabled="true"></asp:TextBox></td>
                        </tr>                    
                    </table>
                </asp:Panel>                              
            </asp:Panel>
            <br /> 
                            
            <br />
                <asp:Button ID="btnProcced" runat="server" Text="Proceed" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" />                     
        </asp:View>
        <asp:View ID="viewRequest" runat="server">
            <table width="100%">
                <tr>
                <td style="width: 379px">
                        <asp:TextBox ID="txtResult" runat="server" Height="274px" Width="1000px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top"><asp:Button ID="BtnEncryptAndSign" runat="server" Text="Encrypt and Sign" />
                    <asp:Button ID="Btnrequest" runat="server" Text="Request" />
                    <asp:Button ID="BtnBack" runat="server" Text="Back" /></td>
                </tr>
                <tr>
                <td style="width: 379px">
                        <asp:TextBox ID="txtResponse" runat="server" Height="274px" Width="627px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr>
                    <td valign="top"><asp:Button ID="BtnDeryptAndVerify" runat="server" Text="Decrypt and verify" /></td> 
                </tr>
            </table>
        </asp:View> 
        <asp:View ID="viewUAT" runat="server">
            <table width="100%">
                <tr>
                     <td><asp:Button ID="btnShowResultXML" runat="server" Text="Show returned XML" /></td>         
                </tr>
                <tr>            
                     <td><asp:TextBox ID="txtUATResponse" runat="server" Height="274px" Width="800px" TextMode="MultiLine" Visible="false"></asp:TextBox></td>
                </tr>
                <tr>
                     <td><asp:TextBox ID="txtExtractedResult" runat="server" Height="274px" Width="800px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>  
                <tr>
                     <td><asp:Button ID="btnUATBack" runat="server" Text="Back" /><asp:Button ID="btnUATBackReset" runat="server" Text="Back and reset" /></td>
                </tr>
            </table>
        </asp:View>   
    </asp:MultiView>

    </form>
</body>
</html>
