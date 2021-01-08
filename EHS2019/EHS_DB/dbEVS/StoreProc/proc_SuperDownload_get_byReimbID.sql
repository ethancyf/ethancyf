
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_SuperDownload_get_byReimbID]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [proc_SuperDownload_get_byReimbID];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-015 special Support Scheme
-- Modified by:	    Martin Tang
-- Modified date:	10 Nov 2020     
-- Description:	  	1. Handle SSSCMC
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:	    Chris YIM
-- Modified date:	07 Feb 2014     
-- Description:	  	1. Remove a row "Total No. of Units Claimed" in [Summary] Page
--					2. Remove a column "No. of Units claimed" in content [01] Page
--					3. Trim the values of "Reimbursement ID" and "Scheme" in [Summary] Page
--					4. Update the wording from "Total Amount" to "Amount Claimed" in [01] and [Summary] Page
--					5. Merge the SP with other SP "proc_SuperDownload_get_byReimbID" into the new SP "proc_ReimbursementFiles_get_byReimbID".
--					   Call sub SP by "File_ID" ( PreAuthorizationCheck, Super ).
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	29 Nov 2012
-- Description:		1. Remove joining by SchemeClaim.Scheme_Seq
--					2. Use [TransactionDetail].[Per_Unit_Value] for SubsidizeValue instead of [SubsidizeGroupClaim].[Subsidize_Value]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			    CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:	    Tommy Tse
-- Modified date:     24 JUL 2012
-- Description:	    Get Display_Code_For_Claim from table SubsidizeGroupClaim
-- =============================================
-- =============================================    
-- Modification History    
-- Modified by:  Derek LEUNG       
-- Modified date:   25 Nov 2010  
-- Description:     Do not include display_seq in order by this will sepearte   
--        CIVSS previous season and current season to 2 sections in summary part  
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Derek LEUNG       
-- Modified date:   21 August 2010  
-- Description:     Fix problem of getting previous season transaction.   
--     Do not get schemeseq by current scheme, use transaction details schemeseq  
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   09 Dec 2009  
-- Description:     Change to get the effective scheme logic  
-- =============================================  
-- =============================================  
-- Author:   Clark Yip  
-- Create date:  29 Jul 2008  
-- Description:  Get Super Donwload File (ych480:Encryption)  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   02 Dec 2008  
-- Description:     Total amount will be calculated based on the Claim_Amount field  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   26 Feb 2009  
-- Description:     The bank_account_no will be selected from transaction table  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   19 Aug 2009  
-- Description:     1. Add Scheme Code  
--     2. Change some heading text  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   03 Sep 2009  
-- Description:     1. Change the super download file with subsidize details in transaction page  
--     2. Add subsidize summary in summary page  
--     3. Check the Subsidize_Type to decide to display the subsidize breakdown  
--     4. Change the "No. of units" in transaction list  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   14 Sep 2009  
-- Description:     1. Rename the subsidize_code column to Subsidy, and show for Vaccine Type only  
--     2. Add subsidy legend in new worksheet page  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   17 Sep 2009  
-- Description:     1. Format the summary worksheet  
--     2. Add scheme legend in legend worksheet page  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Lawrence TSANG  
-- Modified date:   29 September 2009  
-- Description:     Handle expired scheme  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:     Clark Yip  
-- Modified date:   23 Nov 2009  
-- Description:     1. Get the legend description from Subsidize table Subsidize.Legend_Desc  
--     2. Remove the redundant grouping  
--     3. Change the size of Subsidize_Display_Code from 10 to 25  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:       
-- Modified date:     
-- Description:       
-- =============================================  

CREATE PROCEDURE [dbo].[proc_SuperDownload_get_byReimbID] @reimburse_id    CHAR(15), 
                                                          @cutoff_Date_str CHAR(11), 
                                                          @scheme_code     CHAR(10)
AS
    BEGIN

        DECLARE @strReimbursementMode VARCHAR(2);
        DECLARE @strReimbursementModeHAFinance VARCHAR(2)= '2';

        SELECT @strReimbursementMode = Reimbursement_Mode
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

        IF @strReimbursementMode = @strReimbursementModeHAFinance
            BEGIN
                EXEC [proc_ReimbursementFiles_get_byReimbID_SSSCMC] @reimburse_id, 
                                                                    @cutoff_Date_str, 
                                                                    @scheme_code, 
                                                                    'SUPER';
            END;
            ELSE
            BEGIN
                EXEC [proc_ReimbursementFiles_get_byReimbID] @reimburse_id, 
                                                             @cutoff_Date_str, 
                                                             @scheme_code, 
                                                             'SUPER';
            END;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_SuperDownload_get_byReimbID] TO HCVU;
GO