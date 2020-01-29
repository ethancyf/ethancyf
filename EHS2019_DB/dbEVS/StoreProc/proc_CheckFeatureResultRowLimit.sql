IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CheckFeatureResultRowLimit]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CheckFeatureResultRowLimit]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No:
-- Modified by:    
-- Modified date:  
-- Description:    
-- ============================================= 
-- =============================================
-- Author:		Koala CHENG
-- CR No:		CRE12-014
-- Create date: 20 Dec 2012
-- Description:	Check and raise error when @@ROWCOUNT result over the result row limit
-- =============================================
CREATE PROCEDURE proc_CheckFeatureResultRowLimit
	@row_count INT, -- @@ROWCOUNT
	@result_limit_1st_enable BIT, -- 1 = Enable the checking row limit (e.g. 500 rows), 0 = No row limit
	@result_limit_override_enable BIT, -- 1 = Enable the override checking row limit (e.g. 800 rows), 0 = No override
	@override_result_limit BIT  -- 1 = User trigger to relax the checking row limit (e.g. allow over 500 rows but within 4000 rows), 0 = No upper row limit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	
	DECLARE @row_limit_lower AS INT
	DECLARE @row_limit_upper AS INT

    SELECT @row_limit_lower = Parm_Value1, @row_limit_upper = Parm_Value2 
	FROM SystemParameters WITH (NOLOCK) WHERE parameter_name = 'MaxRowRetrieve' AND Record_Status = 'A'  

	-- No result row limit, return without error
	IF @result_limit_1st_enable = 0
		RETURN
	
	-- Any time if row count more than upper limit
	IF @result_limit_override_enable = 1 AND @row_count > @row_limit_upper
	BEGIN
		RAISERROR('00017', 16, 1) -- Result row over the upper limit
		RETURN
	END

	-- lower result row limit enabled
	IF @result_limit_1st_enable = 1 AND @override_result_limit = 0
	BEGIN
		IF @row_count > @row_limit_lower
		BEGIN
			RAISERROR('00009', 16, 1) -- Result row over the lower limit
			RETURN  
		END
	END

	IF @result_limit_1st_enable = 1 AND @override_result_limit = 1
	BEGIN
		IF @row_count > @row_limit_upper
			IF @result_limit_override_enable = 0
				BEGIN
					RAISERROR('00009', 16, 1) -- Result row over the lower limit
					RETURN  
				END
			ELSE
				BEGIN
					RAISERROR('00017', 16, 1) -- Result row over the upper limit
					RETURN  
				END
	END
	
	 
	IF @result_limit_1st_enable = 1 AND @result_limit_override_enable = 0    
	BEGIN    
		IF @row_count > @row_limit_upper    
		BEGIN    
			RAISERROR('00017', 16, 1) -- Result row over the upper limit   
			RETURN   
		END   
		ELSE IF  @row_count > @row_limit_lower    
		BEGIN  
			RAISERROR('00009', 16, 1) -- Result row over the lower limit    
			RETURN  
		END  
	END    


END
GO

GRANT EXECUTE ON [dbo].[proc_CheckFeatureResultRowLimit] TO HCVU
GO
