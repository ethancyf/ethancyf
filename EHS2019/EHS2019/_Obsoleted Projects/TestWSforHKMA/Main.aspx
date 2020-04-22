<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Main.aspx.vb" ValidateRequest="false"
    Inherits="TestWSforHKMA._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Back end interface</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:MultiView ID="mveTest" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewInput" runat="server">
                <asp:Panel ID="SPTestPage" runat="server">
                    <table width="100%">
                        <tr align="right">
                            <td>
                                <a href="testSPLoginPage.htm">Test HCSP Login Page</a></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label ID="Label20" runat="server" Text="Back end interface" Font-Bold="true" />
                <div>
                    <asp:RadioButton ID="rboSPPracticeValidation" runat="server" GroupName="WS_Selection"
                        Text="SP and practice validation" AutoPostBack="true" />
                    <asp:RadioButton ID="rboGetReasonForVisit" runat="server" GroupName="WS_Selection"
                        Text="Get reason for visit list" AutoPostBack="true" />
                    <asp:RadioButton ID="rboRCHNameQuery" runat="server" GroupName="WS_Selection" Text="Get RCH details"
                        AutoPostBack="true" />
                    <asp:RadioButton ID="rboEHSValidatedAccountQuery" runat="server" GroupName="WS_Selection"
                        Text="Query on validated eHealth account" AutoPostBack="true" />
                    <asp:RadioButton ID="rboEHSAccountSubsidyQuery" runat="server" GroupName="WS_Selection"
                        Text="Query on voucher balance" AutoPostBack="true" />
                    <asp:RadioButton ID="rboUploadClaimHL7" runat="server" Checked="True" GroupName="WS_Selection"
                        Text="Upload Claim" BackColor="Aquamarine" AutoPostBack="true" />
                    <asp:RadioButton ID="rboUploadClaim" runat="server" Checked="False" GroupName="WS_Selection"
                        Text="Upload Claim (For Debug Use)" AutoPostBack="true" Visible="false" />
                    <asp:Button ID="btnFreeText" runat="server" Text="Test" Visible="false" />
                </div>
                <br />
                <asp:Panel ID="pnlUATcase" runat="server" BackColor="Info" Width="800">
                    <table>
                        <tr>
                            <td width="210">
                                <label id="lblTextCaseno">
                                    Test Case No
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestCaseNo" runat="server" Text=""></asp:TextBox><asp:Button
                                    ID="btnSearchAndFill" runat="server" Text="Search and Fill" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label id="lblTestCaseDesc">
                                    Test Case Description
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestCaseDesc" runat="server" Enabled="false" Width="600" BackColor="White"></asp:TextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Button ID="ButtonProceedTop" runat="server" Text="Proceed" />
                <asp:Button ID="btnResetTop" runat="server" Text="Reset" />
                <br />
                <br />
                <asp:CheckBox ID="chkSPInfo" runat="server" Text="Service Provider Information" Font-Bold="true"
                    Checked="true" BackColor="Aquamarine" AutoPostBack="true" />
                <asp:Panel ID="pnlSPInfo" runat="server" BackColor="Beige" Width="550">
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:CheckBox ID="chkSPID" runat="server" Text="SP ID" Width="150px" Checked="false"
                                    AutoPostBack="true" Font-Underline="true"></asp:CheckBox></td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtSPID" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkSPSurname" runat="server" Text="SP Surname" Checked="false"
                                    AutoPostBack="true"></asp:CheckBox><asp:CheckBox ID="chkSPGivenname" runat="server"
                                        Text="SP Given Name" Checked="false" AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtSPSurname" runat="server" Enabled="false" BackColor="silver"></asp:TextBox><asp:TextBox
                                    ID="txtSPGivenName" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 26px">
                                <asp:CheckBox ID="chkPracticeID" runat="server" Text="Practice ID" Checked="false"
                                    AutoPostBack="true" Font-Underline="true"></asp:CheckBox></td>
                            <td style="height: 26px">
                                <asp:TextBox ID="txtPracticeID" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkPracticeName" runat="server" Text="Practice Name" Checked="false"
                                    AutoPostBack="true" Font-Underline="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtPracticeName" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkRCHCode" runat="server" Text="RCH Code" AutoPostBack="true"
                                    Visible="false" Font-Underline="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtRCHCode" runat="server" Enabled="false" BackColor="silver" Visible="false"></asp:TextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:CheckBox ID="chkAccountInfo" runat="server" Text="Account Info Information"
                    Font-Bold="true" Checked="true" BackColor="Aquamarine" AutoPostBack="true" />
                <asp:Panel ID="pnlAccountInfo" runat="server" BackColor="Beige" Width="1000">
                    <div style="width: 550">
                        <asp:RadioButton ID="rboHKIC" runat="server" Checked="True" GroupName="DocType" Text="HKIC"
                            AutoPostBack="true" />
                        <asp:RadioButton ID="rboEC" runat="server" GroupName="DocType" Text="EC" AutoPostBack="true" />
                        <asp:RadioButton ID="rboHKBC" runat="server" GroupName="DocType" Text="HKBC" AutoPostBack="true" />
                        <asp:RadioButton ID="rboADOPC" runat="server" GroupName="DocType" Text="ADOPC" AutoPostBack="true" />
                        <asp:RadioButton ID="rboVISA" runat="server" GroupName="DocType" Text="VISA" AutoPostBack="true" />
                        <asp:RadioButton ID="rboID235B" runat="server" GroupName="DocType" Text="ID235B"
                            AutoPostBack="true" />
                        <asp:RadioButton ID="rboDOCI" runat="server" GroupName="DocType" Text="Doc/I" AutoPostBack="true" />
                        <asp:RadioButton ID="rboREPMT" runat="server" GroupName="DocType" Text="REMPT" AutoPostBack="true" />
                    </div>
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <asp:CheckBox ID="chkEntryNo" runat="server" Text="Entry No" Width="150px" AutoPostBack="true" /></td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtEntryNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td style="width: 150px">
                                <asp:CheckBox ID="chkDocumentNo" runat="server" Text="Document No" Width="150px"
                                    AutoPostBack="true"></asp:CheckBox></td>
                            <td style="width: 150px">
                                <asp:TextBox ID="txtDocumentNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkHKIC" runat="server" Text="HKIC" AutoPostBack="true" Font-Underline="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtHKIC" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkRegNo" runat="server" Text="Registration No" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtRegNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkBirthEntryNo" runat="server" Text="Birth Entry No" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtBirthEntryNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkPermitNo" runat="server" Text="Permit No" AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtPermitNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkVISANo" runat="server" Text="VISA No" AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtVISANo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkSurname" runat="server" Text="Surname" AutoPostBack="true" Font-Underline="true">
                                </asp:CheckBox><asp:CheckBox ID="chkGivenName" runat="server" Text="Given Name" AutoPostBack="true"
                                    Font-Underline="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtSurname" runat="server" Enabled="false" BackColor="silver"></asp:TextBox><asp:TextBox
                                    ID="txtGivenName" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkGender" runat="server" Text="Gender" AutoPostBack="true" Font-Underline="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtGender" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkDOB" runat="server" Text="Date of Birth (dd-MM-yyyy)" AutoPostBack="true"
                                    Font-Underline="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtDOB" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkDOBType" runat="server" Text="(DOB Type)" AutoPostBack="true"></asp:CheckBox>
                                <asp:Label ID="Label13" runat="server" ForeColor="Red" Font-Italic="true">Only For EC*</asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtDOBType" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkAgeOn" runat="server" Text="Age On" AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtAgeOn" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkDOReg" runat="server" Text="Date of Registration (dd-MM-yyyy)"
                                    AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtDOReg" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkDOBInWord" runat="server" Text="DOB In Word" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtDOBInWord" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkNameChi" runat="server" Text="Chinese Name" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtNameChi" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkDOI" runat="server" Text="Date of Issue (dd-MM-yyyy)" AutoPostBack="true"
                                    Font-Underline="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtDOI" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkSerialNo" runat="server" Text="Serial No" AutoPostBack="true"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtSerialNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkReference" runat="server" Text="Reference" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtReference" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkFreeReference" runat="server" Text="Free Reference" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtFreeReference" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td>
                                <asp:CheckBox ID="chkRemainUntil" runat="server" Text="Remain Until" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="txtRemainUntil" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:CheckBox ID="chkPassportNo" runat="server" Text="Passport No" AutoPostBack="true">
                                </asp:CheckBox></td>
                            <td valign="top">
                                <asp:TextBox ID="txtPassportNo" runat="server" Enabled="false" BackColor="silver"></asp:TextBox></td>
                            <td colspan="2">
                                <asp:Label ID="lblTip1" runat="server" ForeColor="Red" Font-Italic="true" Width="400">* D:Date of Birth, Y:Year of Birth reported, T:Record on travel document, A:Age on date of registration </asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:CheckBox ID="chkClaimInfo" runat="server" Text="Claim Information" Font-Bold="true"
                    AutoPostBack="true" BackColor="Aquamarine" Checked="true" />
                <asp:Panel ID="pnlClaimInfo" runat="server" BackColor="Beige" Width="800">
                    <table>
                        <tr>
                            <td style="width: 500px">
                                <asp:CheckBox ID="chkServiceDate" runat="server" Text="Service Date (dd-MM-yyyy)"
                                    Width="250px" Enabled="true"></asp:CheckBox>
                                <asp:TextBox ID="txtServiceDate" runat="server" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Label ID="lblVoucher" runat="server" Text="Voucher" Font-Bold="true" Font-Size="Large" />
                                <table style="background-color: White">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:CheckBox ID="chkHCVS" runat="server" Text="Voucher" Width="150px" Enabled="true" /></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkVoucherClaimed" runat="server" Text="Voucher Claimed" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtVoucherClaimed" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkCoPaymentFee" runat="server" Text="Co-Payment Fee" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtCoPaymentFee" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblReasonForVisitPrimary" runat="server" Text="Primary Reason For Visit"></asp:Label>
                                            </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkProfCode" runat="server" Text="Prof Code" Width="150px" Enabled="true">
                                            </asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtProfCode" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkPriorityCode" runat="server" Text="Priority Code" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtPriorityCode" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1Code" runat="server" Text="L1 Code" Width="150px" Enabled="true">
                                            </asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1Code" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1DescEng" runat="server" Text="L1 Desc Eng" Width="150px" Enabled="true">
                                            </asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1DescEng" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkL2Code" runat="server" Text="L2 Code" Width="150px" Enabled="true">
                                            </asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2Code" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td>
                                            <asp:CheckBox ID="chkL2DescEng" runat="server" Text="L2 Desc Eng" Width="150px" Enabled="true">
                                            </asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2DescEng" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -->
                                <table style="background-color: White">
                                    <tr>
                                        <td style="width: 200px">
                                        <asp:Label ID="lblReasonForVisitS1" runat="server" Text="2nd Reason For Visit (S1)"></asp:Label></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkProfCode_S1" runat="server" Text="Prof Code (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtProfCode_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkPriorityCode_S1" runat="server" Text="Priority Code (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtPriorityCode_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1Code_S1" runat="server" Text="L1 Code (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1Code_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1DescEng_S1" runat="server" Text="L1 Desc Eng (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1DescEng_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkL2Code_S1" runat="server" Text="L2 Code (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2Code_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td>
                                            <asp:CheckBox ID="chkL2DescEng_S1" runat="server" Text="L2 Desc Eng (S1)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2DescEng_S1" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <table style="background-color: White">
                                    <tr>
                                        <td style="width: 200px"><asp:Label ID="lblReasonForVisitS2" runat="server" Text="2nd Reason For Visit (S2)"></asp:Label></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkProfCode_S2" runat="server" Text="Prof Code (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtProfCode_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkPriorityCode_S2" runat="server" Text="Priority Code (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtPriorityCode_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1Code_S2" runat="server" Text="L1 Code (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1Code_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1DescEng_S2" runat="server" Text="L1 Desc Eng (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1DescEng_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkL2Code_S2" runat="server" Text="L2 Code (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2Code_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td>
                                            <asp:CheckBox ID="chkL2DescEng_S2" runat="server" Text="L2 Desc Eng (S2)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2DescEng_S2" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <table style="background-color: White">
                                    <tr>
                                        <td style="width: 200px"><asp:Label ID="lblReasonForVisitS3" runat="server" Text="2nd Reason For Visit (S3)"></asp:Label></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkProfCode_S3" runat="server" Text="Prof Code (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtProfCode_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkPriorityCode_S3" runat="server" Text="Priority Code (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtPriorityCode_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1Code_S3" runat="server" Text="L1 Code (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1Code_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkL1DescEng_S3" runat="server" Text="L1 Desc Eng (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtL1DescEng_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkL2Code_S3" runat="server" Text="L2 Code (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2Code_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td>
                                            <asp:CheckBox ID="chkL2DescEng_S3" runat="server" Text="L2 Desc Eng (S3)" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtL2DescEng_S3" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [End] -->
                                <asp:Panel ID="pnlIndicator" runat="server" BackColor="lightgoldenrodyellow">
                                    <table>
                                        <tr>
                                            <td style="width: 150px">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPreSchoolInd" runat="server" Text="Pre School Ind" Width="150px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtPreSchoolInd" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label12" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDoseIntervalInd" runat="server" Text="Dose Interval Ind" Width="150px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtDoseIntervalInd" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label11" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkTSWInd" runat="server" Text="TSW Ind" Width="150px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtTSWInd" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label10" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                                <asp:Button ID="btnAdd2Voucher" runat="server" Text="Add one more entry" /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnl2Voucher" runat="server" Visible="false">
                                    <table style="background-color: White">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkHCVS2" runat="server" Text="Voucher" Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkVoucherClaimed2" runat="server" Text="Voucher Claimed" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtVoucherClaimed2" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkCoPaymentFee2" runat="server" Text="Co-Payment Fee" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtCoPaymentFee2" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkProfCode2" runat="server" Text="Prof Code" Width="150px" Enabled="true">
                                                </asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtProfCode2" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkPriorityCode2" runat="server" Text="Priority Code" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtPriorityCode2" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1Code2" runat="server" Text="L1 Code" Width="150px" Enabled="true">
                                                </asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1Code2" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1DescEng2" runat="server" Text="L1 Desc Eng" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1DescEng2" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkL2Code2" runat="server" Text="L2 Code" Width="150px" Enabled="true">
                                                </asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2Code2" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkL2DescEng2" runat="server" Text="L2 Desc Eng" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2DescEng2" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                    <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -->
                                    <table style="background-color: White">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReasonForVisitS12" runat="server" Text="Reason For Visit S1"
                                                    Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkProfCode_S12" runat="server" Text="Prof Code (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtProfCode_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkPriorityCode_S12" runat="server" Text="Priority Code (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtPriorityCode_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1Code_S12" runat="server" Text="L1 Code (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1Code_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1DescEng_S12" runat="server" Text="L1 Desc Eng (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1DescEng_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkL2Code_S12" runat="server" Text="L2 Code (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2Code_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkL2DescEng_S12" runat="server" Text="L2 Desc Eng (S1)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2DescEng_S12" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                    <table style="background-color: White">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReasonForVisitS22" runat="server" Text="Reason For Visit S2"
                                                    Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkProfCode_S22" runat="server" Text="Prof Code (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtProfCode_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkPriorityCode_S22" runat="server" Text="Priority Code (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtPriorityCode_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1Code_S22" runat="server" Text="L1 Code (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1Code_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1DescEng_S22" runat="server" Text="L1 Desc Eng (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1DescEng_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkL2Code_S22" runat="server" Text="L2 Code (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2Code_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkL2DescEng_S22" runat="server" Text="L2 Desc Eng (S2)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2DescEng_S22" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                    <table style="background-color: White">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReasonForVisitS32" runat="server" Text="Reason For Visit S3"
                                                    Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkProfCode_S32" runat="server" Text="Prof Code (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtProfCode_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkPriorityCode_S32" runat="server" Text="Priority Code (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtPriorityCode_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1Code_S32" runat="server" Text="L1 Code (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1Code_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkL1DescEng_S32" runat="server" Text="L1 Desc Eng (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtL1DescEng_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkL2Code_S32" runat="server" Text="L2 Code (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2Code_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkL2DescEng_S32" runat="server" Text="L2 Desc Eng (S3)" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtL2DescEng_S32" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                    <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [End] -->
                                    <asp:Panel ID="pnlIndicatorVoucher2" runat="server" BackColor="lightgoldenrodyellow">
                                        <table>
                                            <tr>
                                                <td style="width: 150px">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPreSchoolInd2" runat="server" Text="Pre School Ind" Width="150px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtPreSchoolInd2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="Label9" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkDoseIntervalInd2" runat="server" Text="Dose Interval Ind" Width="150px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtDoseIntervalInd2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="Label8" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkTSWInd2" runat="server" Text="TSW Ind" Width="150px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtTSWInd2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="Label7" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                                <td>
                                                    <asp:Button ID="btnRemove2Voucher" runat="server" Text="Remove 2nd entry" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <br />
                                <asp:Label ID="lblVaccination" runat="server" Text="Vaccination" Font-Bold="true"
                                    Font-Size="Large" />
                                <table style="background-color: White">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSchemeCode" runat="server" Text="+ Scheme Code" Width="150px" /></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtSchemeCode" runat="server" Width="150px" Enabled="true"></asp:TextBox></td>
                                        <td>
                                            <asp:CheckBox ID="chkRCHCode_Vaccine1" runat="server" Text="RCH Code"></asp:CheckBox></td>
                                        <td>
                                            <asp:TextBox ID="txtRCHCode_Vaccine1" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkVaccine1" runat="server" Text="Vaccine 1" Width="150px" Enabled="true" /></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkSubsidyCode1" runat="server" Text="Subsidy Code" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtSubsidyCode1" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkDoseSeq1" runat="server" Text="Dose Seq" Width="80px" Enabled="true">
                                            </asp:CheckBox>
                                            <asp:Label ID="Label18" runat="server" ForeColor="Red" Font-Italic="true">**</asp:Label></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtDoseSeq1" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkVaccine2" runat="server" Text="Vaccine 2" Width="150px" Enabled="true" /></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkSubsidyCode2" runat="server" Text="Subsidy Code" Width="150px"
                                                Enabled="true"></asp:CheckBox></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtSubsidyCode2" runat="server" Enabled="true"></asp:TextBox></td>
                                        <td style="width: 150px">
                                            <asp:CheckBox ID="chkDoseSeq2" runat="server" Text="Dose Seq" Width="80px" Enabled="true">
                                            </asp:CheckBox>
                                            <asp:Label ID="Label19" runat="server" ForeColor="Red" Font-Italic="true">**</asp:Label></td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtDoseSeq2" runat="server" Enabled="true"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Panel1" runat="server" BackColor="lightgoldenrodyellow">
                                    <table>
                                        <tr>
                                            <td style="width: 150px">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPreSchoolInd_vaccine" runat="server" Text="Pre School Ind" Width="155px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtPreSchoolInd_vaccine" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label4" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDoseIntervalInd_vaccine" runat="server" Text="Dose Interval Ind"
                                                    Width="150px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtDoseIntervalInd_vaccine" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label5" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkTSWInd_vaccine" runat="server" Text="TSW Ind" Width="150px" /></td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtTSWInd_vaccine" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                <asp:Label ID="Label6" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                            <td>
                                                <asp:Button ID="btnAdd2Vaccine" runat="server" Text="Add one more entry" /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnl2Vaccine" runat="server" Visible="false">
                                    <table style="background-color: White">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="+ Scheme Code" Width="150px" /></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtSchemeCode2" runat="server" Width="150px" Enabled="true"></asp:TextBox></td>
                                            <td>
                                                <asp:CheckBox ID="chkRCHCode_Vaccine2" runat="server" Text="RCH Code"></asp:CheckBox></td>
                                            <td>
                                                <asp:TextBox ID="txtRCHCode_Vaccine2" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkVaccine21" runat="server" Text="Vaccine 1" Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkSubsidyCode21" runat="server" Text="Subsidy Code" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtSubsidyCode21" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkDoseSeq21" runat="server" Text="Dose Seq" Width="80px" Enabled="true">
                                                </asp:CheckBox>
                                                <asp:Label ID="Label17" runat="server" ForeColor="Red" Font-Italic="true">**</asp:Label></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtDoseSeq21" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkVaccine22" runat="server" Text="Vaccine 2" Width="150px" Enabled="true" /></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkSubsidyCode22" runat="server" Text="Subsidy Code" Width="150px"
                                                    Enabled="true"></asp:CheckBox></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtSubsidyCode22" runat="server" Enabled="true"></asp:TextBox></td>
                                            <td style="width: 150px">
                                                <asp:CheckBox ID="chkDoseSeq22" runat="server" Text="Dose Seq" Width="80px" Enabled="true">
                                                </asp:CheckBox>
                                                <asp:Label ID="Label16" runat="server" ForeColor="Red" Font-Italic="true">**</asp:Label></td>
                                            <td style="width: 150px">
                                                <asp:TextBox ID="txtDoseSeq22" runat="server" Enabled="true"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="Panel2" runat="server" BackColor="lightgoldenrodyellow">
                                        <table>
                                            <tr>
                                                <td style="width: 150px">
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPreSchoolInd_vaccine2" runat="server" Text="Pre School Ind"
                                                        Width="155px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtPreSchoolInd_vaccine2" runat="server" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="tip" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkDoseIntervalInd_vaccine2" runat="server" Text="Dose Interval Ind"
                                                        Width="150px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtDoseIntervalInd_vaccine2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkTSWInd_vaccine2" runat="server" Text="TSW Ind" Width="150px" /></td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtTSWInd_vaccine2" runat="server" Width="150px" Enabled="true"></asp:TextBox>
                                                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Font-Italic="true">Y or N</asp:Label></td>
                                                <td>
                                                    <asp:Button ID="btnRemove2Vaccine" runat="server" Text="Remove 2nd Entry" /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label ID="lblTip2" runat="server" ForeColor="Red" Font-Italic="true">** 1ST: First dose, 2ND: Second dose, N/A: N/A dose </asp:Label>
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
                        <td valign="top">
                            <asp:Button ID="BtnEncryptAndSign" runat="server" Text="Encrypt and Sign" />
                            <asp:Button ID="Btnrequest" runat="server" Text="Request" />
                            <asp:Button ID="BtnBack" runat="server" Text="Back" /></td>
                    </tr>
                    <tr>
                        <td style="width: 379px">
                            <asp:TextBox ID="txtResponse" runat="server" Height="274px" Width="627px" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Button ID="BtnDeryptAndVerify" runat="server" Text="Decrypt and verify" /></td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="viewUAT" runat="server">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnShowResultXML" runat="server" Text="Show returned XML" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUATResponse" runat="server" Height="274px" Width="800px" TextMode="MultiLine"
                                Visible="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtExtractedResult" runat="server" Height="274px" Width="800px"
                                TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnUATBack" runat="server" Text="Back" /><asp:Button ID="btnUATBackReset"
                                runat="server" Text="Back and reset" /></td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>
