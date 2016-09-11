
declare @Temp  TABLE(
	Name VARCHAR(100),
	OutcomePattern int,
	Predicate VARCHAR(1000)
)

INSERT INTO @Temp (Name,OutcomePattern,Predicate)
select [Column 0],CONVERT(int,[Column 1]),[Column 2]  
from build

DECLARE @Count int
declare @Name varchar(100),
	@OutcomePattern int,
	@Predicate varchar(1000),
	@PredicateID int
	
select @Count = COUNT(1) from @Temp

WHILE(@Count > 0)
begin
	select top 1 @Name = Name,@OutcomePattern = OutcomePattern,@Predicate = Predicate from @Temp
	
	insert into Predicate (Name,OutcomePattern,ModuleID)
	values (@Name,@OutcomePattern,1)
	
	select top 1 @PredicateID = ID from Predicate where Name = @Name
	
	insert into Parameter (Value,PredicateID)
	select *,@PredicateID from dbo.fnSplit(@Predicate,',')
	
	delete from @Temp where Name = @Name
	
	select @Count = COUNT(1) from @Temp
end

--select * from Predicate
--select * from Parameter