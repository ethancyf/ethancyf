
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_PreAuthorizationCheck_get_byReimbID]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [proc_PreAuthorizationCheck_get_byReimbID];
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
-- Description:	  	1.  Handle Reimbursement_Mode = 2 (SSSCMC)
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
-- Modified By:  Derek LEUNG              
-- Modified date: 15 September 2010              
-- Description:  update to meet new report standard          
-- =============================================              
-- =============================================      
-- Modification History      
-- Modified by:  Derek LEUNG         
-- Modified date:   21 August 2010    
-- Description:     Fix problem of getting previous season transaction.     
--     Do not get schemeseq by current scheme, use transaction details schemeseq    
-- =============================================      
-- =============================================        
-- Author:   Derek LEUNG      
-- Create date:  28 June 2010      
-- Description:        
-- =============================================        
CREATE PROCEDURE [dbo].[proc_PreAuthorizationCheck_get_byReimbID] @reimburse_id    CHAR(15), 
                                                                  @cutoff_Date_str CHAR(11), 
                                                                  @scheme_code     CHAR(10)
AS
    BEGIN

        DECLARE @strReimbursement_Mode VARCHAR(2);
        SELECT @strReimbursement_Mode = Reimbursement_Mode
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

        IF @strReimbursement_Mode = '2'
            BEGIN
                EXEC proc_ReimbursementFiles_get_byReimbID_SSSCMC @reimburse_id, 
                                                                  @cutoff_Date_str, 
                                                                  @scheme_code, 
                                                                  'PreAuthorizationCheck';
            END;
            ELSE
            BEGIN
                EXEC proc_ReimbursementFiles_get_byReimbID @reimburse_id, 
                                                           @cutoff_Date_str, 
                                                           @scheme_code, 
                                                           'PreAuthorizationCheck';
            END;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_PreAuthorizationCheck_get_byReimbID] TO HCVU;
GO