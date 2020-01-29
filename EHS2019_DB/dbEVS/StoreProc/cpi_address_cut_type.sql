IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_cut_type]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_cut_type]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[cpi_address_cut_type] (@in_name varchar(50), @out_name varchar(50) output )
AS
begin
	declare @record_type varchar(20)
	select @in_name = upper(@in_name)
   if (charindex(space(1), @in_name) > 0)
   begin
      -- Cut the last word for record type checking
      select @record_type = reverse(substring(reverse(ltrim(rtrim(@in_name))), 1, 
												charindex(' ', reverse(ltrim(rtrim(@in_name))))-1))
      -- If the last word is found in the record type table, cut it off
      if exists (select * from address_type 
									where full_name = @record_type or abbreviation = @record_type)
         select @in_name = reverse(substring(reverse(ltrim(rtrim(@in_name))), 
						charindex(' ', reverse(ltrim(rtrim(@in_name))))+1, 50))
   end
	select @out_name = ltrim(rtrim(@in_name))
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_type] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_type] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_type] TO HCVU
GO
