
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummary_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ReimbursementSummary_get]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	17 Aug 2020
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
--					1. SQL Tuning
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 March 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- CR #:			INT15-0001
-- Modified by:		Karl LAM	
-- Modified date:	05 Mar 2015
-- Description:		Add with recompile to avoid (Thread is being aborted error)
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Summary
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   17 Aug 2009
-- Description:	    Adopt to new reimbursement schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Retrieve [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   07 Sep 2009
-- Description:	    Combine the select statements into 1
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSummary_get] @cutoff_dtm  DATETIME, 
                                                       @scheme_code CHAR(10)
AS
    BEGIN
        --***********************************************/
        --* Declaration                                 */                        
        --***********************************************/
        DECLARE @txn_tot AS INT;
        DECLARE @distinct_sp_count AS INT;
        DECLARE @voucher_tot AS INT;
        DECLARE @amt_tot AS MONEY;

		DECLARE @In_cutoff_dtm  DATETIME = @cutoff_dtm
        DECLARE @In_scheme_code CHAR(10) = @scheme_code      

        --***********************************************/
        --*   Validation                                */
        --***********************************************/
        --***********************************************/
        --*   Initialization                            */
        --***********************************************/

        SELECT @txn_tot = COUNT(1)
        FROM VoucherTransaction AS t
             LEFT OUTER JOIN ReimbursementAuthTran AS rt
             ON t.transaction_id = rt.transaction_id
        WHERE t.Record_status = 'A'
              AND rt.Authorised_status IS NULL
              AND t.Confirmed_dtm <= @In_cutoff_dtm
              AND t.Scheme_code = @In_scheme_code
              AND rt.Scheme_code IS NULL;

        -- =============================================
        -- Return results
        -- =============================================
        IF @txn_tot > 0
            BEGIN

                SELECT @txn_tot AS noTran, 
                       COUNT(DISTINCT CAST(t.SP_ID AS VARCHAR) + ' (' + CAST(t.practice_display_seq AS VARCHAR) + ')') AS noSP, 
                       SUM(TD.Unit) AS vouchersClaimed, 
                       SUM(TD.Total_Amount) AS totalAmount, 
                       SUM(TD.Total_Amount_RMB) AS [TotalAmountRMB]
                FROM VoucherTransaction AS T
                     LEFT OUTER JOIN ReimbursementAuthTran AS rt
                     ON T.transaction_id = rt.transaction_id
                     INNER JOIN TransactionDetail AS TD
                     ON T.Transaction_ID = TD.Transaction_ID
                WHERE t.Record_status = 'A'
                      AND rt.Authorised_status IS NULL
                      AND t.Confirmed_dtm <= @In_cutoff_dtm
                      AND t.Scheme_code = @In_scheme_code
                      AND rt.Scheme_code IS NULL;
            END;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummary_get] TO HCVU;
GO