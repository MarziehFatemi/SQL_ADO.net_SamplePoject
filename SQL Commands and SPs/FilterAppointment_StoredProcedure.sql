use hiexpert2
go
create procedure FilterAppointment 
@PriceFrom NVARCHAR(30) ,
@PriceTo NVARCHAR(30) ,
@Cust NVARCHAR(30) ,         
@Dr NVARCHAR (30) , 
@StartDate DATETIME,
@EndDate DATETIME
as 
 DECLARE @DrFilteredTB TABLE
                (
                 OrderID int,
                 Cus NChar(50),
                 Dr NChar(50),
                 DateofVisit DateTime,
                 Payment int

                )
if (@Dr != '')
                    Begin
                    INSERT INTO @DrFilteredTB SELECT* FROM Appointment where Physician = @Dr;
                end
else
                    Begin
                    INSERT INTO @DrFilteredTB SELECT* FROM Appointment
                    end

DECLARE @CusFilteredTB TABLE
(
 OrderID int,
 Cus NChar(50),
 Dr NChar(50),
 DateofVisit DateTime,
 Payment int

)
if (@Cust != '')
                    Begin
                    INSERT INTO @CusFilteredTB SELECT* FROM @DrFilteredTB cus where cus.Cus = @Cust;
                end
else
                    begin
                    INSERT INTO @CusFilteredTB SELECT* FROM @DrFilteredTB
                    end
--SELECT * FROM  @CusFilteredTB
DECLARE @PriceFilteredTB TABLE
(
 OrderID int,
 Cus NChar(50),
 Dr NChar(50),
 DateofVisit DateTime,
 Payment int
 
)
if ((@PriceFrom != '') and (@PriceTo != ''))
                    Begin
                    INSERT INTO @PriceFilteredTB SELECT* FROM @CusFilteredTB FCust 
                    where FCust.Payment >= @PriceFrom and FCust.Payment <= @PriceTo ;
                end
else
                    begin
                    INSERT INTO @PriceFilteredTB SELECT* FROM @CusFilteredTB
                    end


DECLARE @DateFilteredTB TABLE
(
 OrderID int,
 Cus NChar(50),
 Dr NChar(50),
 DateofVisit DateTime,
 Payment int

)
if ( (@StartDate != '') AND (@EndDate != ''))
Begin
INSERT INTO @DateFilteredTB SELECT* FROM @PriceFilteredTB PTable 
where PTable.DateofVisit >= @StartDate
and  PTable.DateofVisit <= @EndDate;
end
else
                    begin
                    INSERT INTO @DateFilteredTB SELECT* FROM @PriceFilteredTB
                    end

SELECT * FROM @DateFilteredTB 
go 
