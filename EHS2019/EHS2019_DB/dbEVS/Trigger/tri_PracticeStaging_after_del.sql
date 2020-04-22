IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_PracticeStaging_after_del')
	DROP TRIGGER [dbo].[tri_PracticeStaging_after_del] + Environment.NewLineGO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 23 Jun 2009
-- Description:	Trigger an delete and update statment on PracticeMigration
--				when a row is deleted from PracticeStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark YIP	
-- Modified date:	28 Jul 2009
-- Description:		Add back the logic only Abort / delete of newly added practice
--					will trigger the delete of practicetransition and migration
-- =============================================

CREATE TRIGGER [dbo].[tri_PracticeStaging_after_del]
   ON  [dbo].[PracticeStaging]
   AFTER DELETE
AS 
BEGIN

	SET NOCOUNT ON;
		
	Declare @encrypt_field1	varbinary(100)
	Declare @display_seq smallint
	Declare @enrolment_ref_no char(15)
	Declare @record_status char(1)
	Declare @counter as smallint
	Declare @spid as char(8)

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	select @enrolment_ref_no = Enrolment_Ref_No from deleted
	
	select @encrypt_field1= 
	(Select sp.encrypt_field1 from ServiceProvider sp where sp.enrolment_ref_no=@enrolment_ref_no
		union
	Select sps.encrypt_field1 from ServiceProviderStaging sps where sps.enrolment_ref_no=@enrolment_ref_no)
	
	select @display_seq = display_seq from deleted
	
	select @record_status = Record_Status from deleted
	
	select @spid = sp_id from ServiceProvider where enrolment_ref_no=@enrolment_ref_no
	
	select @counter = count(1) from Practice where Display_Seq = @display_seq and sp_id = @spid
--The record status is not a filtering criteria in this part, since
--1. Production case: delete a newly added pratice, record status will be A (Not Exist in permanent table)
--2. Production case: "Abort" a new enrolment / sub-sequent updtae, record status will be R (Not Exist in permanent table)
--3. Production case: Complete a new enrolment, record status will be A (Exist in permanent table)
if (@counter=0)
BEGIN
	DELETE	FROM PracticeTransition
	WHERE	Encrypt_Field1 = @encrypt_field1 and
			Display_Seq = @display_seq			

	UPDATE	PracticeTransition
	SET		Display_Seq = Display_Seq - 1
	WHERE	Display_Seq > @display_seq and
			Encrypt_Field1 = @encrypt_field1
			
	DELETE	FROM PracticeMigration
	WHERE	enrolment_ref_no=@enrolment_ref_no and
			Display_Seq = @display_seq

	UPDATE	PracticeMigration
	SET		Display_Seq = Display_Seq - 1
	WHERE	Display_Seq > @display_seq and
			enrolment_ref_no=@enrolment_ref_no
	
	if (@record_status='R')
	BEGIN
		delete from practicetransition
		where sp_id = @spid and 
		display_Seq < @display_Seq and display_Seq not in 
		(select display_seq from practice
		where display_seq < @display_Seq and sp_id = @spid)
		
		DELETE	FROM PracticeMigration
		WHERE	enrolment_ref_no=@enrolment_ref_no and 
		display_Seq < @display_Seq and display_Seq not in 
		(select display_seq from practice
		where display_seq < @display_Seq and sp_id = @spid)
			
	END	
END

CLOSE SYMMETRIC KEY sym_Key

END
GO
