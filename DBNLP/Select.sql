use DBNLP
go
select * from Module
select * from Predicate
select * from Parameter
select * from OutcomeLabel
select * from OutcomePattern
select * from Items
select * from build (nolock)


declare @Module int
declare @Predicate int
declare @Parameter int
declare @OutcomeLabel int
declare @OutcomePattern int
declare @Items int
declare @Temp int


select @Module = COUNT(1) from Module
select @Predicate = COUNT(1) from Predicate
select @Parameter = COUNT(1) from Parameter
select @OutcomeLabel  = COUNT(1) from OutcomeLabel
select @OutcomePattern = COUNT(1) from OutcomePattern
select @Items= COUNT(1) from Items
select @Temp= COUNT(1) from Temp

select @Module as Module,@Predicate as Predicate,@Parameter as Parameter,@OutcomeLabel as OutcomeLabel,@OutcomePattern as OutcomePattern,@Items as Items,@Temp as Temp

SELECT m.ID,m.Name,pd.Name,pd.OutcomePattern,pm.Value
FROM Module m 
INNER JOIN Predicate pd ON m.ID = pd.ModuleID
INNER JOIN Parameter pm ON pd.ID = pm.PredicateID