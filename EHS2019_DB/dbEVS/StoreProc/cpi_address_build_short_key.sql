IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_build_short_key]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_build_short_key]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[cpi_address_build_short_key] (@search_name varchar(255), @short_key char(7) output )
AS
begin
declare @post int
declare @word1 char(50)
declare @word2 char(50)
declare @word3 char(50)
declare @key char(1)
select @word1 = ''
select @word2 = ''
select @word3 = ''
select @key = ''
select @search_name = ltrim(rtrim(upper(@search_name)))
	/* Copy the first word if exist */
	select @post = charindex( ' ', @search_name)
	if not (@post = 0)
	begin
		select @word1 = substring(@search_name, 1 ,@post - 1)
		select @search_name = substring(@search_name, @post + 1, 255)
		/* Copy the second word if exist */
		select @post = charindex( ' ', @search_name)
		if not (@post = 0)
		begin
			select @word2 = substring(@search_name, 1, @post - 1)
			select @search_name = substring(@search_name, @post + 1, 255)
			/* Copy the third word if exist */
			select @post = charindex( ' ', @search_name)
			if not (@post = 0)
			begin
				select @word3 = substring(@search_name, 1, @post - 1)
				select @search_name = substring(@search_name, @post + 1, 255)
			end
			else
				select @word3 = @search_name
		end
		else
		begin
			select @word2 = @search_name
			select @word3 = space(50)
		end
	end
	else
	begin
		select @word1 = @search_name
		select @word2 = space(50)
		select @word3 = space(50)
	end
	/* Start to make the short key */
	select @short_key = substring(@word1, 1, 2) + 
				substring(@word2, 1, 1) +
				substring(@word3, 1, 1) + 
				substring(@word1, 3, 1) + 
				substring(@word2, 2, 1) + 
				substring(@word3, 2, 1)
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_short_key] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_short_key] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_short_key] TO HCVU
GO
