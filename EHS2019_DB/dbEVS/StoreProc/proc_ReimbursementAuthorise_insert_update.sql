IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorise_insert_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorise_insert_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Authorisation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

/*
CREATE PROCEDURE 	[dbo].[proc_ReimbursementAuthorise_insert_update] @tran_status		 char(1)
							,@cutoff_dtm			datetime
							,@authorised_dtm		datetime
							,@authorised_status		char(1)
							,@authorised_by	      	varchar(20)		
							,@current_user			varchar(20)
							,@first_authorized_dtm	datetime
							,@first_authorized_by	varchar(20)
							,@scheme_code			char(10)
as
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare   @tran_id    as  char(20)
declare	  @tran_tot	  as  int

-- =============================================
-- Validation 
-- =============================================

-- =============================================
-- Initialization
-- =============================================

-- =============================================
-- Return results
-- =============================================

DECLARE authorise_cursor CURSOR FOR 
SELECT t.transaction_id
FROM VoucherTransaction t, reimbursementAuthTran rt WHERE t.Record_status = 'A' and 
((@authorised_status='1' and t.Authorised_status is null) OR (@authorised_status='2' and t.Authorised_status='1'
and DATEDIFF("mi", @first_authorized_dtm, rt.first_authorised_dtm) = 0
and rt.first_authorised_by = @first_authorized_by))
and t.transaction_dtm <= @cutoff_dtm
and t.scheme_code = @scheme_code
and t.transaction_id = rt.transaction_id

OPEN authorise_cursor
FETCH NEXT FROM authorise_cursor INTO @tran_id

WHILE @@FETCH_STATUS = 0
BEGIN

select @tran_tot = count(1) from ReimbursementAuthTran 
where transaction_id = @tran_id

if @tran_tot = 0
BEGIN
	--First Authorisation
INSERT INTO [dbEVS].[dbo].[ReimbursementAuthTran]
           ([Transaction_ID]
           ,[First_Authorised_Dtm]
		   ,[First_Authorised_By]
           ,[Authorised_Status])
     VALUES
           (@tran_id
           ,@authorised_dtm
		   ,@authorised_by
           ,@authorised_status)
END
ELSE
BEGIN
	--Second Authorisation
	UPDATE [dbEVS].[dbo].[ReimbursementAuthTran]
   SET [Second_Authorised_Dtm] = @authorised_dtm
	  ,[Second_Authorised_By] = @authorised_by
      ,[Authorised_Status] = @authorised_status
 WHERE [Transaction_ID]=@tran_id	
END

--Update the record status in VoucherTransaction table
UPDATE [dbEVS].[dbo].[VoucherTransaction]
   SET [Authorised_status] = @authorised_status
 WHERE [Transaction_ID]=@tran_id

FETCH NEXT FROM authorise_cursor INTO @tran_id
END
CLOSE authorise_cursor
DEALLOCATE authorise_cursor

INSERT INTO [dbEVS].[dbo].[ReimbursementAuthorisation]
           ([Authorised_Dtm]
           ,[Authorised_Status]
           ,[Authorised_By]           
           ,[Record_Status]
           ,[Create_By]
           ,[Create_Dtm]
		   ,[Update_By]
		   ,[Update_Dtm])
     VALUES
           (@authorised_dtm
           ,@authorised_status
           ,@authorised_by           
           ,@tran_status
           ,@current_user
           ,{fn now()}
		   ,@current_user
           ,{fn now()})
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorise_insert_update] TO HCVU
GO
*/
