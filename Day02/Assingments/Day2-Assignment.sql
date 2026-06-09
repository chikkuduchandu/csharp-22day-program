--Assignment1
select 
	p.FullName as ProviderName,
	d.Name as DepartmentName,
	count(e.EncounterId) as TotalEncounters,
	rank() over(order by count(e.EncounterId) desc )as ProviderRank
from 
	Provider p
	join
	Department d
	on p.DepartmentId=d.DepartmentId
	left join
	Encounter e
	on p.ProviderId=e.ProviderId
group by
	p.ProviderId,
	p.FullName,
	d.Name



--Assignment2


ALTER TABLE Insurance
ADD
    ValidFrom DATETIME2
        GENERATED ALWAYS AS ROW START HIDDEN
        CONSTRAINT DF_Insurance_Provider_From
        DEFAULT SYSUTCDATETIME(),

    ValidTo DATETIME2
        GENERATED ALWAYS AS ROW END HIDDEN
        CONSTRAINT DF_Insurance_Provider_To
        DEFAULT '9999-12-31 23:59:59.9999999',

    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);


ALTER TABLE Insurance
SET (
    SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = dbo.Insurance_Provider_History
    )
);

UPDATE Insurance
SET Payer='HDFC'
WHERE PatientId = 1;

SELECT
    InsuranceId,
    Payer,
    PolicyNumber,
	ValidFrom,
    ValidTo
FROM Insurance
FOR SYSTEM_TIME ALL
WHERE PatientId = 1 
ORDER BY ValidFrom;

--Assignment3

create or alter procedure Claim_Dashboard
as
begin
with Claim_Summary as(
select
	Status,
	count(claimId) as TotalClaims,
	sum(BilledAmount)  as TotalBilledAmount,
	sum(coalesce(ReimbursedAmt,0))  as TotalReimbursedAmt,
	sum(BilledAmount)-sum(coalesce(ReimbursedAmt,0)) as OutstandingAmt

from 
	Claim 

group by
	status
)
select
	status,
	TotalBilledAmount,
	TotalReimbursedAmt,
	OutstandingAmt,
	rank() over(order by OutstandingAmt Desc) Rank
from Claim_Summary
end;

exec Claim_Dashboard;


--Assignment3
create or alter procedure Dashboard as
begin
select count(*) from Patient where IsActive=1;
select 
		top 5 d.Name as DepartmentName,count(e.EncounterId) as NoOfEncounters
from
	Encounter e
	join
	Department d
	on e.DepartmentId=d.DepartmentId
group by
	d.DepartmentId,d.Name
Order by
	count(e.EncounterId) desc

select avg(datediff(Day,AdmitDate,DischargeDate))
from Encounter
where DischargeDate is not null
end;

exec Dashboard;

exec 