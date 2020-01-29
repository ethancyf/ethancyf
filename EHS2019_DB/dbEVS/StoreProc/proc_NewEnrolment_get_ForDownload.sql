IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_NewEnrolment_get_ForDownload]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_NewEnrolment_get_ForDownload]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 14 Aug 2008
-- Description:	Retrieve New Enrolment For Download
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_NewEnrolment_get_ForDownload]
	@PeriodFrom datetime,
	@PeriodTo datetime
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

DECLARE	
		@record_id int,
		@display_seq smallint,
		
		@address_eng varchar(255),
		@address_chi varchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255)
		
DECLARE @tempNewEnrolment Table 
(
	SP_ID char(8),
	Effective_Dtm datetime,
	Display_Seq smallint,
	Practice_Name nvarchar(100),
	District char(5),
	Profession varchar(100),
	Address_Code int
)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

INSERT INTO @tempNewEnrolment
(
	SP_ID,
	Effective_Dtm,
	Display_Seq,
	Practice_Name,
	District,
	Profession,
	Address_Code
)
SELECT
	SP.SP_ID as SP_ID,
	SP.Effective_Dtm as Effective_Dtm,
	P.Display_Seq as Display_Seq,
	P.Practice_Name as Practice_Name,
	P.District as District,
	SD.Data_Value as Profession,
	P.Address_Code as Address_Code
FROM
	[dbo].[ServiceProvider] SP
		LEFT OUTER JOIN [dbo].[Practice] P ON
			SP.SP_ID = P.SP_ID
		LEFT OUTER JOIN [dbo].[Professional] Prof ON
			P.SP_ID = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
		LEFT OUTER JOIN [dbo].[StaticData] SD
			ON SD.Column_Name = 'PROFESSION' AND SD.Item_No = Prof.Service_Category_Code
WHERE 
	SP.Effective_Dtm < DateAdd(Day,1,@PeriodTo) AND SP.Effective_Dtm >= @PeriodFrom
	
ORDER BY SP.Effective_Dtm ASC, SP.SP_ID, P.Display_Seq ASC

	
----  LOOK UP DISTRICT -------

DECLARE cur cursor FOR
	SELECT Address_Code, Display_Seq  FROM @tempNewEnrolment
	
OPEN cur
FETCH NEXT FROM cur INTO @record_id, @display_seq
WHILE @@Fetch_status = 0
BEGIN
	IF @record_id IS NOT NULL
	BEGIN
	
		SELECT @address_eng = '', @address_chi = '', @district_code = '', @eh_eng = '', @eh_chi = ''

		exec cpi_get_address_detail
			@record_id,
			@address_eng = @address_eng OUTPUT,
			@address_chi = @address_chi OUTPUT,
			@district_code = @district_code OUTPUT,
			@eh_eng = @eh_eng OUTPUT,
			@eh_chi = @eh_chi OUTPUT

		UPDATE @tempNewEnrolment
			SET	District = @district_code
		WHERE Display_Seq = @display_seq
		
	END
	
	FETCH NEXT FROM cur INTO @record_id, @display_seq
END
CLOSE cur
DEALLOCATE cur

----------------- SELECT Result ----------------------

SELECT
	SP_ID as [HCSP ID],
	ISNULL(Effective_Dtm,'') as [Enrollment Date Time],
	ISNULL(Display_Seq,'') as [Practice No.],
	ISNULL(Practice_Name,'') as [Practice Name],
	ISNULL(District,'') as [Practice District],
	ISNULL(Profession,'') as [Profession]
FROM
	@tempNewEnrolment NE
		LEFT OUTER JOIN [dbo].[District] D
			ON NE.District = D.district_code
	
ORDER BY NE.Effective_Dtm ASC, NE.SP_ID ASC, NE.Display_Seq ASC
	
--------------- SELECT Parameter -------------------

SELECT @PeriodFrom as [Period From], @PeriodTo as [Period To]
		
END

GO

GRANT EXECUTE ON [dbo].[proc_NewEnrolment_get_ForDownload] TO HCVU
GO
