IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VaccineRemark_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_VaccineRemark_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	08 Oct 2019
-- CR No.:			CRE19-001 (New initiatives for VSS in 2019-20)
-- Description:		Pt.(B)3. not include MMR
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	2 November 2015
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Remove line break for Section 4: DOB Flag
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			CRE14-021 Use of IV for Southern Hemisphere Vaccine under RVP
-- Modified by:		Chris YIM
-- Modified date:	24 April 2015
-- Description:		1. remove description in B3
-- =============================================  
-- =============================================
-- Modification History
-- CR No.:			CRE14-021 Use of IV for Southern Hemisphere Vaccine under RVP
-- Modified by:	    Winnie SUEN
-- Modified date:   11 Mar 2015
-- Description:	    1. Change Legend Part 1,2,3 to dynamic gen
-- =============================================   
-- =============================================
-- Modification History
-- CR No.:			CRE14-017-04
-- Modified by:	    Winnie SUEN
-- Modified date:   27 Jan 2015
-- Description:	    1. Remove EHAPP & PCV13 in Part 4
--					2. Add OMPCV13E information
-- =============================================   
-- =============================================
-- Modification History
-- CR No.:			INT14-0032
-- Modified by:	    Karl LAM
-- Modified date:   12 Nov 2014
-- Description:	    1. Update for vaccination new season
-- =============================================   
-- =============================================
-- Modification History
-- CR No.:			CRE13-017-05 CVSSPCV13
-- Modified by:	    Karl LAM
-- Modified date:   04 DEC 2013
-- Description:	    1. Add CVSSPCV13 information
--					2. Select to eHSD0025-CVSSPCV13 Claim  
--					3. Add subsidy 13/14 to CSIV, ESIV,RSIV,RSIV-HCW
-- =============================================   
-- =============================================
-- Modification History
-- CR No.:			CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:	    Tommy Tse
-- Modified date:   03 Sep 2012
-- Description:	    1. Generate whole remark page
--					2. Get Display_Code_For_Claim From Table SubsidizeGroupClaim
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:  Eric Tse  
-- Modified date: 27 October 2010  
-- Description:  New procedure to select vaccination remark.  
--     this procedure will share with  
--     1.eHSD0002-EVSS Claim  
--     2.eHSD0003-CIVSS Claim  
--     3.eHSD0004-RVP Claim  
-- =============================================    
CREATE PROCEDURE  [dbo].[proc_EHS_VaccineRemark_Stat_Read]    
  
as  
BEGIN  
  
--***********************************************/    
--* Declaration                                 */                            
--***********************************************/   
DECLARE @Seq INT
CREATE TABLE #tblRemark (
	Seq	INT,
	Result_Value1 NVARCHAR(MAX),    
	Result_Value2 NVARCHAR(MAX)  
)
--***********************************************/    
--*   Validation                                */    
--***********************************************/    
--***********************************************/    
--*   Initialization                            */    
--***********************************************/   
 SET @Seq = 0;
--***********************************************/    
--*   Return results                            */    
--***********************************************/  

--(A)1. Lengend
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '(A) Legend', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '1. Identity Document Type', NULL
SET @Seq = @Seq + 1;

INSERT INTO #tblRemark
SELECT @Seq, Doc_Display_Code, Doc_Name
FROM DocType
ORDER BY Display_Seq

SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL


--(A)2. Scheme
SET @Seq = @Seq + 1;

INSERT INTO #tblRemark
SELECT @Seq, '2. Scheme', NULL

SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, Display_Code, Scheme_Desc
FROM SchemeClaim
ORDER BY Scheme_Code

SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL


--(A)3. Subsidy
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '3. Subsidy', NULL

SET @Seq = @Seq + 1;
INSERT INTO 
	#tblRemark
SELECT DISTINCT 
	@Seq, Display_Code_For_Claim, Legend_Desc_For_Claim
FROM 
	SubsidizeGroupClaim a
		INNER JOIN Subsidize b 
			ON a.Subsidize_Code = b.Subsidize_Code
		INNER JOIN Subsidizeitem c 
			ON b.subsidize_item_Code = c.Subsidize_Item_Code AND c.Subsidize_Type = 'VACCINE'
ORDER BY 
	Display_Code_For_Claim

SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL


--(A)4. DOB Flag
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '4. DOB Flag', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'D', 'Exact date DD/MM/YYYY'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'M', 'MM/YYYY'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'Y', 'Only year YYYY'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'A', 'Exemption Certificate: Date of registration + age'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'R', 'Exemption Certificate: Reported year of birth'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'T', 'Exemption Certificate: Exact date DD/MM/YYYY on travel document' + '   ' + 'HKBC: Exact date DD/MM/YYYY for DOB in word'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'U', 'Exemption Certificate: MM/YYYY on travel document' + '   ' + 'HKBC: MM/YYYY for DOB in word'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, 'V', 'Exemption Certificate: Only year YYYY on travel document' + '   ' + 'HKBC: Only year YYYY for DOB in word'
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL

--(B) Common Note

--(B)1. eHealth Accounts
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '(B) Common Note(s) for the report', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '1. eHealth (Subsidies) Accounts:', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   a. eHealth (Subsidies) Account is one with same document type (except HKIC and HKBC) and identity document no..', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   b. Multiple accounts with same document type and identity document no. is counted as one eHealth (Subsidies) Account.', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   c. HKIC and HKBC with same identity document no. will be counted as same account.', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL


-- (B)2. Transactions
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '2. Transactions:', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated.', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '   c. Exclude voided/deleted transactions.', NULL
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, NULL, NULL

-- (B)3. All are accumulative data unless specified as below:
SET @Seq = @Seq + 1;
INSERT INTO #tblRemark
SELECT @Seq, '3. All are accumulative data unless specified as below:', NULL

SET @Seq = @Seq + 1;
INSERT INTO 
	#tblRemark
SELECT 
	@Seq, RTRIM(a.Display_Code_For_Claim) + ' : ' + REPLACE(CONVERT(VARCHAR(12),CONVERT(DATETIME,Claim_Period_From ),106),' ',' '), NULL
FROM 
	SubsidizeGroupClaim a    
		INNER JOIN SubsidizeGroupClaimItemDetails b   
			ON a.scheme_code = b.scheme_code and a.scheme_seq = b.scheme_seq 
				AND a.subsidize_code = b.subsidize_code    
				AND b.Subsidize_Item_Code NOT IN ('PV', 'EHAPP_R', 'PV13', 'MMR')
WHERE 
	b.record_status = 'A'    
GROUP BY 
	a.Display_Code_For_Claim,a.scheme_seq, Claim_Period_From    
ORDER BY 
	a.Display_Code_For_Claim,a.scheme_seq     
    
SELECT Result_Value1, Result_Value2 FROM #tblRemark ORDER BY Seq

DROP TABLE #tblRemark   
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_VaccineRemark_Stat_Read] TO HCVU
GO


