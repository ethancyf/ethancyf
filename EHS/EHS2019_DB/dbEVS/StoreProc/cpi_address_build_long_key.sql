IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_build_long_key]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_build_long_key]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[cpi_address_build_long_key] (@search_name varchar(255), @long_key varchar(24) output )
AS
begin
	select @search_name = upper(@search_name)
	while charindex(space(1), rtrim(@search_name)) != 0
   select @search_name = 
      stuff(@search_name, charindex(space(1), @search_name), 1, null)
	/* Delete all the space and get the leftmost 24 characters */
	select @long_key = substring(@search_name, 1,24)
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_long_key] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_long_key] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_long_key] TO HCVU
GO
