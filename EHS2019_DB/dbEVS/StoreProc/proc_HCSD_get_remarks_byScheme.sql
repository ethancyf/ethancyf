IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_remarks_byScheme]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_remarks_byScheme]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:			Dickson Law
-- Create date:		11 Dec 2017
-- Description:		Change SDIR remark display by scheme code
--					Store Proc rename from proc_HCSD_get_remarks_byProf
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSD_get_remarks_byScheme] 
	@scheme_item varchar(20)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @delimiter VARCHAR(5)  
	 
	DECLARE @tblScheme_Group TABLE(
		Num INT,  
		Scheme_Group VARCHAR(10)  
	)  

	DECLARE @RemarkResult TABLE(
		Scheme          char(10),
		Num				char(5),
		Description		varchar(510),
		Description_Chi varchar(510)
   ) 

-- =============================================
-- Initialization
-- =============================================	

	SET @delimiter = ';'  

	INSERT 
		@tblScheme_Group
		(
			Num,
			Scheme_Group
		) 
	SELECT 
		ROW_NUMBER() OVER (ORDER by ITEM),
		ITEM 
	FROM 
		func_split_string(@scheme_item, @delimiter) 
	ORDER BY ITEM

-- =============================================
-- Retrieve Data
-- =============================================	

	DECLARE @i INT
	DECLARE @totalScheme INT
	
	SET @i = 1
	SELECT @totalScheme = COUNT(Scheme_Group) FROM @tblScheme_Group

	-- Create remark result table of selected scheme   
	WHILE @i <= @totalScheme  BEGIN

		DECLARE @schemeNow char(10)

		SELECT 
			@schemeNow = Scheme_Group
		FROM
			@tblScheme_Group
		WHERE 
			NUM = @i

		--Header of different scheme remark 
		INSERT INTO @RemarkResult
		(
			Scheme,
			Num,
			Description,
			Description_Chi
		)
		SELECT
			SDS.Scheme_Code
			,'0'
			,Scheme_Desc
			,Scheme_Desc_Chi 
		FROM 
			@tblScheme_Group SG
		INNER JOIN 
			SDScheme SDS 
				ON SG.Scheme_Group = SDS.Scheme_Code
		WHERE 
			SDS.Scheme_Code = @schemeNow
		
		--Different scheme remark
		INSERT INTO @RemarkResult
		(
			Scheme,
			Num,
			Description,
			Description_Chi
		)
		SELECT 
			@schemeNow,
			ROW_NUMBER() OVER(ORDER BY Display_Seq),
			Description,  
			Description_Chi
		FROM  
			SDRemarks   
		WHERE  
			 (Scheme_code = 'ALL' OR Scheme_code = @schemeNow) 
			 AND GETDATE() >= Effective_Dtm  
			 AND GETDATE() < Expiry_Dtm  
			 AND Record_Status = 'A'  
		ORDER BY  
			Display_Seq  
		
		SET @i = @i+1
	END
 
-- =============================================
-- Return results
-- =============================================

	SELECT * FROM @RemarkResult
		

		
END	
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_remarks_byScheme] TO HCPUBLIC
GO
