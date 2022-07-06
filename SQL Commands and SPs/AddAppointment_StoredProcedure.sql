use Hiexpert2
Go 

CREATE PROCEDURE Filter 
@price int, @Cust varchar(30),@Dr Varchar (30),@DrShare int,@Date DAteTime
AS

declare @ID as int = (select Max(AppointmentID) + 1 from Appointment);

insert into Appointment(AppointmentID, Physician, Patient, DateOfVisit, Payment)
values(@ID, @Dr, @Cust, @Date, @Price)

declare @CustTotalPayment as int = 0;
declare @DrTotalSale as int = 0;
declare @DrTotalIncome as int = 0;


if ((select TotalPayments from Patient where UserName = @Cust) is not null ) 
begin
set  @CustTotalPayment = (select TotalPayments from Patient where UserName = @Cust);
end
else
begin
set @CustTotalPayment = 0;
end

if ((select TotalSale from Physician where UserName = @Dr) is not null ) 
begin
set  @DrTotalSale = (select TotalSale from Physician where UserName = @Dr);
end
else
begin
set @DrTotalSale = 0;
end



if ((select CommisionPercent from Physician where UserName = @Dr) is not null ) 
begin
set  @DrShare = (select CommisionPercent from Physician where UserName = @Dr);
end



                    update Patient set TotalPayments = @CustTotalPayment + @price where USERNAME = @Cust;

                    set @DrTotalSale = @DrTotalSale + @price;
                    update Physician set Totalsale = @DrTotalSale where USERNAME = @Dr;

                    update Physician set TotalIncome = (@DrShare * @DrTotalSale / 100) where USERNAME = @Dr;
Go 
