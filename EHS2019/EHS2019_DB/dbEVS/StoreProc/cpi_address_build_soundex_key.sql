IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_build_soundex_key]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_build_soundex_key]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[cpi_address_build_soundex_key] ( @name varchar(255),  @soundex_key  varchar(8) output)
AS
begin
declare @counter int
declare @last_char varchar(1)
declare @word1 varchar (255)
declare @word2 varchar(255)
declare @temp_word varchar(255) 
declare @temp_soundex_key varchar(16) 
declare @post int 
declare @soundex_chk int, @temp_char char(1) 
 
/*****   24 Nov 2003: Prevent 'Null' string Concaterated with Char String   ****/
-- select   @word1 = null
-- select   @word2 = null
select  @word1 = ''
	,@word2 = ''
select   @soundex_key = ''
select   @post = 0    
select 	@name = rtrim(upper(@name))
select @post = charindex( ' ', @name)
if not (@post = 0)
begin
	select @word1 = substring(@name, 1 ,@post - 1)
	select @name = substring(@name, @post + 1, 255)
	/* Copy the second word if exist */
	select @post = charindex( ' ', @name)
	if not (@post = 0)
	begin
		select @word2 = substring(@name, 1, @post - 1)
		select @name = substring(@name, @post + 1, 255)
	end
	else
		select @word2 = @name
end
else
begin
	select @word1 = @name
	select @word2 = null
end
--print @word1
--print @word2
select @temp_word = @word1
select   @counter = 1
while isnull(len(@soundex_key),0) <= 7
begin       
	if @temp_word is null
	begin
		select @soundex_key = rtrim(@soundex_key) + '0000'
		continue
	end	
	if  @counter = 1
	begin 
   	if (substring(@temp_word, @counter, 1) IN ('0','1','2','3','4','5','6','7','8','9'))
		begin  
			select @soundex_key = rtrim(@soundex_key) + '0000'
			select @temp_word = @word2
			select @counter = 1   
			continue
		end
		else
		begin
			select @soundex_key = rtrim(@soundex_key) + substring(@temp_word, @counter, 1)
			select @last_char =  substring(@temp_word, @counter, 1)
			select @counter = @counter + 1
		end
	end
	else
	begin 
		while  (@counter <= 255)
		begin
			if (len(@soundex_key)=4  and  (@word1 = @temp_word)) or  
						(len(@soundex_key)=8  and  (@word2 = @temp_word))
				break
			if (@last_char <>  substring(@temp_word, @counter, 1))
			begin
				select @temp_char = null
				select @temp_char = phonetic_code from address_phonetic_code 
								where phonetic_char = substring(@temp_word, @counter, 1)
			/********           24 Nov 2004: Handle the 'Empty String' case  also    *********/
			--	if @temp_char is not null 
			if  (@temp_char is not null ) or  ltrim(rtrim(@temp_char)) = ''
				begin
					select @soundex_key = rtrim(@soundex_key) + @temp_char
--print @temp_char
--print @soundex_key
					select @last_char =  substring(@temp_word, @counter, 1)
				end
			end
			select  @counter = @counter + 1
		end
		if len(@soundex_key) <= 4
		begin
			select @temp_soundex_key = @soundex_key + '0000'
			select @soundex_key = substring(@temp_soundex_key,1,4)
			select @temp_word = @word2
			select @counter = 1
			continue
		end
		if len(@soundex_key) > 4 and len(@soundex_key) <= 8
		begin
			select @temp_soundex_key = @soundex_key + '0000'
			select @soundex_key = substring(@temp_soundex_key,1,8)
			break
		end
	end  
                                          
end
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_soundex_key] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_soundex_key] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_build_soundex_key] TO HCVU
GO
