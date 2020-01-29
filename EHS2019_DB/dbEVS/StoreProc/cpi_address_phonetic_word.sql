IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_phonetic_word]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_phonetic_word]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[cpi_address_phonetic_word] (@string varchar(255), @output varchar(255) output )
AS
begin
declare @word varchar(100)
declare @i int
/*****   24 Nov 2003: Prevent 'Null' string Concaterated with Char String   ****/
--select @output = null
--select @word = null
select  @word = ''
	 ,@output = ''
select @i = 1
if @string is not null
begin
	select @string = upper(@string)
	select @string = ltrim(rtrim(@string))
	while 0=0
	begin
		select @i = charindex(space(1), @string)
		if @i > 0
		begin
			select @word = substring(@string, 1, @i - 1)
			select @string = substring(@string, @i + 1, len(@string))
		end
		else
		begin
			select @word = @string
			select @string = null
		end
		-- reselect @i for other operation use
		select @i = 0
		-- Get the standard word from the table and add it to the output string
		select @word = standard from address_phonetic_word where non_standard = @word
		select @output = @output + ltrim(rtrim(@word)) + space(1)
		-- when the string is empty, that means the whole string has been converted
		if @string is null break
	end
end
	select @output = ltrim(rtrim(@output))
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_phonetic_word] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_phonetic_word] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_phonetic_word] TO HCVU
GO
