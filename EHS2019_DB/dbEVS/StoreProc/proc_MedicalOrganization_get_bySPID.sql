IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganization_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganization_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE16-009 (To enable the eHS(S) to display Chinese Unicode in the field of "English Name of M.O.")
-- Modified by:		Koala CHENG
-- Modified date:	07 Sep 2016
-- Description:		Change MO_Eng_Name from VARCHAR(100) to NVARCHAR(100)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 07 April 2009
-- Description:	Retrieve the Medical Organization from Table
--				MedicalOrganization
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Lawrence TSANG
-- Modified date: 13 May 2009
-- Description:	  (1) Change field names "MO_Relation" --> "Relationship" 
--                    and "MO_Relation_Remark" --> "Relationship_Remark"
--                (2) Add field "Record_Status"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].[proc_MedicalOrganization_get_bySPID] 
	@sp_id	char(8)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)

declare @display_seq smallint
		
DECLARE @tmp_MO table ( SP_ID	char(8),
						Display_Seq	smallint,
						--SP_Practice_Display_Seq	smallint,
						MO_Eng_Name	nvarchar(100),
						MO_Chi_Name	nvarchar(100),
						Room	nvarchar(5),
						[Floor]	nvarchar(3),
						Block	nvarchar(3),
						Building	varchar(100),
						Building_Chi	nvarchar(100),
						District	char(4),
						Address_Code	int,
						BR_Code	varchar(50),
						Phone_Daytime	varchar(20),
						Email	varchar(255),
						Fax	varchar(20),
						Relationship char(5),
						Relationship_Remark nvarchar(255),
						Record_Status char(1),
						create_by varchar(20),
						create_dtm datetime,
						update_by varchar(20),
						update_dtm datetime,
						tsmp timestamp)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	INSERT INTO @tmp_MO (SP_ID,
						Display_Seq,
						--SP_Practice_Display_Seq,
						MO_Eng_Name,
						MO_Chi_Name,
						Room,
						[Floor],
						Block,
						Building,
						Building_Chi,
						District,
						Address_Code,
						BR_Code,
						Phone_Daytime,
						Email,
						Fax,
						Relationship,
						Relationship_Remark,
						Record_Status,
						create_by,
						create_dtm,
						update_by,
						update_dtm)

	SELECT  SP_ID,
			Display_Seq, 
			--SP_Practice_Display_Seq, 
			MO_Eng_Name, 
			MO_Chi_Name, 
			Room, 
			[Floor],
			Block, 
			Building, 
			Building_Chi, 
			District, 
			Address_Code, 
			BR_Code, 
			Phone_Daytime, 
			Email, 
			Fax,
			Relationship,
			Relationship_Remark,
			Record_Status,
			create_by,
			create_dtm,
			update_by,
			update_dtm
	FROM    MedicalOrganization
	WHERE	SP_ID = @sp_id
	
DECLARE avail_cursor cursor 
FOR	SELECT Address_Code, Display_Seq
FROM @tmp_MO

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE @tmp_MO
	SET	Building = @address_eng, 
		Building_Chi = @address_chi,
		District = @district_code
	WHERE Display_Seq = @display_seq
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq
END
CLOSE avail_cursor 
DEALLOCATE avail_cursor


	SELECT  t.SP_ID,
			t.Display_Seq, 
			--SP_Practice_Display_Seq, 
			t.MO_Eng_Name, 
			t.MO_Chi_Name, 
			t.Room, 
			t.[Floor],
			t.Block, 
			t.Building, 
			t.Building_Chi, 
			t.District, 
			t.Address_Code, 
			t.BR_Code, 
			t.Phone_Daytime, 
			isNull(t.Email,'') as Email, 
			t.Fax,
			t.Relationship,
			t.Relationship_Remark,
			t.Record_Status,
			t.create_by,
			t.create_dtm,
			t.update_by,
			t.update_dtm,
			m.tsmp
	FROM    @tmp_MO t, medicalorganization m
	WHERE	t.sp_id = m.sp_id
	and		t.SP_ID = @sp_id
	and		t.display_seq = m.display_seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_get_bySPID] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganization_get_bySPID] TO WSINT
GO