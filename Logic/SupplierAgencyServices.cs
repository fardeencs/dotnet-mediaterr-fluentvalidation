namespace Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL;
    using Domain;
    using Logic.Interface;
    using System.Transactions;
    using Domain.Entity;
    using System;
    using Domain.Enumrations;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Xml.Linq;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Configuration;
    using System.Globalization;

    public class SupplierAgencyServices : ISupplierAgencyServices
    {

        public async Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetails(string agencyCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = await (from sm in _ctx.tblSupplierMasters
                                                  join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                                  //join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals                                  sapi.SupplierId
                                                  where sm.Status == status && ag.AgencyCode == agencyCode
                                                  select new SupplierAgencyDetails
                                                  {
                                                      AccountNumber = sm.AccountNumber,
                                                      AgencyID = ag.AgencyID,
                                                      Password = sm.Password,
                                                      Status = sm.Status,
                                                      SupplierCode = sm.SupplierCode,
                                                      SupplierName = sm.SupplierName,
                                                      UserName = sm.UserName,
                                                  }).ToListAsync();

                return supplierBasicDetails;

            }
        }

        public async Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetailswithsuppliercode(string agencyCode, string status, string supplierCode)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = await (from sm in _ctx.tblSupplierMasters
                                                  join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                                  //join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals                                  sapi.SupplierId
                                                  where sm.Status == status && ag.AgencyCode == agencyCode && sm.SupplierCode == supplierCode
                                                  select new SupplierAgencyDetails
                                                  {
                                                      AccountNumber = sm.AccountNumber,
                                                      AgencyID = ag.AgencyID,
                                                      Password = sm.Password,
                                                      Status = sm.Status,
                                                      SupplierCode = sm.SupplierCode,
                                                      SupplierName = sm.SupplierName,
                                                      UserName = sm.UserName,
                                                  }).ToListAsync();

                return supplierBasicDetails;

            }
        }

        public SupplierAgencyDetails GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string routeFlag)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals sapi.SupplierId
                                            where sm.Status == "T" && ag.AgencyCode == agencyCode && sapi.SupplierRoute == routeFlag && sm.SupplierCode == supplierCode
                                            select new SupplierAgencyDetails
                                            {
                                                BaseUrl = sm.BaseUrl,
                                                RequestUrl = sapi.RequestUrl,
                                                AgencyID = ag.AgencyID,
                                                SupplierId = sm.SupplierId,
                                            }).FirstOrDefault();

                return supplierBasicDetails;

            }
        }

        public async Task<BookingData> BookFlights(string emailId, long agencyId, string mobileNo, string bookingRfNo, long supplierId)
        {
            string bookingid = "";
            BookingData _BookingData = new BookingData();
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    var exsitUser = _ctx.tblUsers.FirstOrDefault(x => x.EmailID == emailId);
                    using (var scope = new TransactionScope())
                    {
                        if (exsitUser == null)
                        {
                            tblUser user = new tblUser()
                            {
                                EmailID = emailId,
                                CreatedBy = 1,
                                Mobile = mobileNo,
                                AgencyID = agencyId,
                                LoginStatus = 1,
                                AdminStatus = 0
                            };
                            _ctx.tblUsers.Add(user);
                            await _ctx.SaveChangesAsync();

                            bookingid = await InsertOrUpdateBooking(agencyId, bookingRfNo, supplierId, _ctx, user.UserID);
                            await InsertBookingHistory(_ctx, bookingid, user.UserID);
                        }
                        else
                        {
                            bookingid = await InsertOrUpdateBooking(agencyId, bookingRfNo, supplierId, _ctx, exsitUser.UserID);

                            await InsertBookingHistory(_ctx, bookingid, exsitUser.UserID);


                        }

                        scope.Complete();

                    }
                    _BookingData.BookingRefID = Convert.ToInt64(bookingid);
                    _BookingData.userID = Convert.ToInt64(exsitUser.UserID);
                }

            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //return "000000";
            }
            catch (Exception ex)
            {
                //return "000000";
            }
            return _BookingData;
        }

        public async Task<string> InsertOrUpdateBooking(long agencyId, string bookingRfNo, long supplierId,
            MediationEntities _ctx, long userId)
        {
            await Task.Delay(0);
            string bookingrfid;

            try
            {
                tblBooking booking = new tblBooking()
                {
                    BookingID = 1,
                    Position = 1,
                    UserID = userId,
                    ServiceTypeId = 1,
                    BookingDate = DateTime.Now,
                    SupplierID = supplierId,
                    SupplierBookingReference = bookingRfNo,
                    CancellationDeadline = DateTime.Now,
                    BookingStatusCode = "RQ",
                    AgencyRemarks = "",
                    PaymentStatusID = 1,
                };

                _ctx.tblBookings.Add(booking);
                _ctx.SaveChanges();

                booking.BookingID = booking.BookingRefID;


                _ctx.Entry(booking).State = EntityState.Modified;
                _ctx.SaveChanges();
                bookingrfid = booking.BookingRefID.ToString();
                return bookingrfid;
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return "000000";
            }
            catch (Exception ex)
            {
                return "000000";
            }


        }

        public async Task InsertBookingHistory(MediationEntities _ctx, string bookingid, long userID)
        {
            await Task.Delay(0);
            string bookingrfid;

            try
            {
                tblBookingHistory bookinghistory = new tblBookingHistory()
                {
                    BookingRefID = Convert.ToInt64(bookingid.ToString()),
                    BookingStatusCode = "RQ",
                    UserID = userID,
                    ActionDate = DateTime.Now



                };

                _ctx.tblBookingHistorys.Add(bookinghistory);
                _ctx.SaveChanges();







            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
            catch (Exception ex)
            {

            }


        }

        public async Task AddAirPassengers(List<Travelerinfo> message, string bookingId, string emailid, string telphone, string locationcode, string UserID)
        {
            bool isUpdated = true;
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    var xDoc = GetXDocTravelInfoProcesses(message, bookingId, emailid, telphone, locationcode, UserID);

                    const string sql = @"EXEC spAddAirPassengersupdatedNew @xmlData";
                    var result = await _ctx.Database
                        .SqlQuery<SpBaseEntity>(
                                  sql
                                , new SqlParameter("xmlData", new SqlXml(xDoc.Root.CreateReader()))

                        ).FirstOrDefaultAsync();

                    if (result.StatusCode != "OK-200")
                    {
                        isUpdated = false;
                    }
                    // return isUpdated;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task AddAirbookingCost(List<Travelerinfo> message, string bookingid)
        {
            int paxcount = message.Count;
            using (var _ctx = new MediationEntities())

            {


                tblAirPassengers addpassenger = new tblAirPassengers();

                {





                    for (int i = 0; i > paxcount; i++)

                    {



                    }
                }
            }
        }

        //public async Task<bool> AddSupplierResponse(AgencySupplierResponseEntity message)
        //{
        //    bool isUpdated = false;
        //    try
        //    {
        //        message.SearchDateTime = DateTime.Now;
        //        using (var _ctx = new MediationEntities())
        //        {
        //            _ctx.tblAgencySeacrhDetails.Add(new tblAgencySeacrhDetail()
        //            {
        //                AgencyCode = message.AgencyCode,
        //                ArrDate = message.ArrDate,
        //                DepDate = message.DepDate,
        //                FromLoc = message.FromLoc,
        //                ToLoc = message.ToLoc,
        //                SearchDateTime = message.SearchDateTime,
        //                Status = message.Status,
        //                SupplierCode = message.SupplierCode,
        //                Search = message.Search,
        //                ADT=message.ADT,
        //                CHD=message.CHD,
        //                INF=message.INF,
        //                Class=message.Class
        //            });
        //            await _ctx.SaveChangesAsync();
        //            isUpdated = true;
        //            return isUpdated;
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {

        //        var outputLines = new List<string>();
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
        //            }
        //        }
        //        return isUpdated;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
        public async Task<bool> AddSupplierResponse(List<AgencySupplierResponseEntity> message)
        {
            bool isUpdated = false;
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    message.ForEach(_ =>
                    {
                        _ctx.tblAgencySeacrhDetails.Add(new tblAgencySeacrhDetail()
                        {
                            AgencyCode = _.AgencyCode,

                            SearchDateTime = _.SearchDateTime,
                            Status = _.Status,
                            SupplierCode = _.SupplierCode,
                            Search = _.Search,
                            Key = _.Key

                        });
                    });

                    await _ctx.SaveChangesAsync();

                    isUpdated = true;
                    return isUpdated;
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return isUpdated;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<string> GetAgencySupplierResponse(AgencySupplierResponseEntity message)
        {
            try
            {
                message.SearchDateTime = DateTime.Now;
                int minToAdd = 20;
                using (var _ctx = new MediationEntities())
                {
                    const string sql = @"EXEC [dbo].[spGetAgencySupplierResponse]
                                                 @AgencyCode
				                                ,@SearchDateTime
				                                ,@Key
				                                ,@Status
				                                ,@SupplierCode
                                                ,@MinutesToAdd
                                                ";
                    //var result = await _ctx.Database
                    //   .SqlQuery<SpBaseEntity>(
                    //             sql
                    //           , new SqlParameter("AgencyCode", message.AgencyCode)
                    //           , new SqlParameter("SearchDateTime", message.SearchDateTime)
                    //           , new SqlParameter("Key", message.Key)
                    //           , new SqlParameter("Status", message.Status)
                    //           , new SqlParameter("SupplierCode", message.SupplierCode)
                    //           , new SqlParameter("MinutesToAdd", minToAdd)
                    //   ).FirstOrDefaultAsync();

                    var result = _ctx.Database
                        .SqlQuery<SpBaseEntity>(
                                  sql
                                , new SqlParameter("AgencyCode", message.AgencyCode)
                                , new SqlParameter("SearchDateTime", message.SearchDateTime)
                                , new SqlParameter("Key", message.Key)
                                , new SqlParameter("Status", message.Status)
                                , new SqlParameter("SupplierCode", message.SupplierCode)
                                , new SqlParameter("MinutesToAdd", minToAdd)
                        ).FirstOrDefault();

                    if (result == null)
                        result.Data = null;

                    if (result.StatusCode != "OK-200")
                        result.Data = null;

                    return result.Data;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        //public async Task<string> GetAgencySupplierResponse(List<AgencySupplierResponseEntity> message, string supplierCode)
        //{
        //    try
        //    {
        //        message.First().SearchDateTime = DateTime.Now;
        //        int minToAdd = 20;
        //        using (var _ctx = new MediationEntities())
        //        {
        //            const string sql = @"EXEC [dbo].[spGetAgencySupplierResponse]
        //                                         @AgencyID
        //                            ,@SearchDateTime
        //                            ,@FromLoc
        //                            ,@ToLoc
        //                            ,@DepDate
        //                            ,@ArrDate
        //                            ,@Status
        //                            ,@SupplierCode
        //                                        ,@MinutesToAdd
        //                                        ";
        //            var result = _ctx.Database
        //                .SqlQuery<SpBaseEntity>(
        //                          sql
        //                        , new SqlParameter("AgencyID", message.First().AgencyID)
        //                        //, new SqlParameter("SearchDateTime", message.SearchDateTime)
        //                        //, new SqlParameter("FromLoc", message.FromLoc)
        //                        //, new SqlParameter("ToLoc", message.ToLoc)
        //                        //, new SqlParameter("DepDate", message.DepDate)
        //                        //, new SqlParameter("ArrDate", message.ArrDate)
        //                        , new SqlParameter("Status", message.First().Status)
        //                        , new SqlParameter("SupplierCode", message.First().SupplierCode)
        //                        , new SqlParameter("MinutesToAdd", minToAdd)

        //                ).FirstOrDefault();

        //            if (result.StatusCode != "OK-200")
        //            {
        //                result.Data = null;
        //            }
        //            return result.Data;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}


        private static XDocument GetXDocTravelInfoProcesses(List<Travelerinfo> list, string bookingId, string emailid, string telphone, string locationcode, string UserID)
        {
            return new XDocument(
                new XElement("EVENTS",
                    from xevent in list
                    select new XElement("EVENT"
                            , new XElement("GivenName", xevent.GivenName)
                            , new XElement("BookingRefID", bookingId)
                            , new XElement("TitleID", GetTitleID(xevent.NamePrefix))
                            , new XElement("PaxTypeID", Getpaxtype(xevent.PassengerTypeQuantity))
                            , new XElement("Surname", xevent.Surname)
                            , new XElement("EmailID", emailid)
                            , new XElement("TelPhoneType", telphone)
                            , new XElement("LocationCode", locationcode)
                            , new XElement("DateofBirth", xevent.BirthDate)
                            , new XElement("PNR", "")
                            , new XElement("ETicketNo", "")
                            , new XElement("UserID", UserID)
                            , new XElement("DocTypeID", getDocId(xevent._DocType))
                            , new XElement("DocumentNumber", xevent._DocID)
                            , new XElement("ExpiryDate", DateTime.Now)
                            , new XElement("IssueLocation", "")
                            , new XElement("IssueCountry", xevent._DocIssueCountry)
                        )));
        }
        public static string getDocId(string doctype)
        {
            string typeid = "";
            doctype = doctype.ToLower();
            if (doctype == "passport")
            {
                typeid = "1";
            }
            else if (doctype == "govt issued Identity card")
            {
                typeid = "2";
            }

            return typeid;
        }
        public static string GetTitleID(string title)
        {
            string titleID = "";
            if (title == "Mr.")
            {
                titleID = "1";
            }
            else if (title == "Mrs.")
            {
                titleID = "2";
            }
            else if (title == "Ms.")
            {
                titleID = "3";
            }
            else if (title == "Miss.")
            {
                titleID = "4";
            }
            else if (title == "Mstr.")
            {
                titleID = "5";
            }
            else if (title == "Dr.")
            {
                titleID = "6";
            }
            else if (title == "HE.")
            {
                titleID = "7";
            }
            else if (title == "HH.")
            {
                titleID = "8";
            }
            return titleID;

        }
        public static string Getpaxtype(string paxtype)
        {
            string paxtypeID = "1";
            if (paxtype == "ADT")
            {
                paxtypeID = "1";
            }
            else if (paxtype == "CHD")
            {
                paxtypeID = "2";
            }
            else if (paxtype == "INF")
            {
                paxtypeID = "3";
            }
            return paxtypeID;
        }
        //Azharudheen new
        //tblAirBookingCost
        public async Task<string> InsertIntotblairbookingcost(string BookingRefID, double TotalBaseNet,
            double TotalTaxNet, double TotalNet, string NetCurrency,
            int MarkupTypeID, double MarkupValue, string MarkupCurrency, double SellAmount,
            string SellCurrency, double AdditionalServiceFee, double CancellationAmount, string CancellationCurrency)
        {
            string BookingCostID = "";
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    tblAirBookingCost _tblAirBookingCost = new tblAirBookingCost()
                    {
                        BookingRefID = Convert.ToInt64(BookingRefID),
                        TotalBaseNet = Convert.ToDecimal(TotalBaseNet),
                        TotalTaxNet = Convert.ToDecimal(TotalTaxNet),
                        TotalNet = Convert.ToDecimal(TotalNet),
                        NetCurrency = NetCurrency,
                        MarkupTypeID = MarkupTypeID,
                        MarkupValue = Convert.ToDouble(MarkupValue),
                        MarkupCurrency = MarkupCurrency,
                        SellAmount = Convert.ToDecimal(SellAmount),
                        SellCurrency = SellCurrency,
                        AdditionalServiceFee = Convert.ToDecimal(AdditionalServiceFee),
                        CancellationAmount = Convert.ToDecimal(CancellationAmount),
                        CancellationCurrency = CancellationCurrency
                    };

                    _ctx.tblAirBookingCost.Add(_tblAirBookingCost);
                    _ctx.SaveChanges();
                    BookingCostID = _tblAirBookingCost.BookingCostID.ToString();
                }

            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
            return BookingCostID;
        }
        //tblAirBookingCostBreakup
        public async Task InsertIntotblAirBookingCostBreakup(string bookingCostID, List<CostBreakuppax> costBreakuppax)
        {
            try
            {
                int totalpaxtype = costBreakuppax.Count;
                for (int i = 0; i < totalpaxtype; i++)
                {
                    string baseNet = costBreakuppax[i].PaxtotalBaseNet;
                    string taxNet = costBreakuppax[i].PaxtotalTaxNet;
                    string totalNet = costBreakuppax[i].PaxpaidAmount;
                    string netCurrency = costBreakuppax[i].PaxnetCurrency;
                    int markupTypeID = Convert.ToInt16(costBreakuppax[i].PaxmarkupTypeID);
                    double markupValue = Convert.ToDouble(costBreakuppax[i].PaxmarkupValue);
                    string markupCurrency = costBreakuppax[i].PaxmarkupCurrency;
                    double sellAmount = Convert.ToDouble(costBreakuppax[i].PaxsellAmount);
                    string sellCurrency = costBreakuppax[i].PaxsellCurrency;
                    double additionalServiceFee = Convert.ToDouble(costBreakuppax[i].PaxadditionalServiceFee);
                    string PaxType = costBreakuppax[i].PaxType;
                    int NoOfPAx = Convert.ToInt16(costBreakuppax[i].TotalPaxQuantity);
                    using (var _ctx = new MediationEntities())
                    {
                        tblAirBookingCostBreakup _tblAirBookingCostBreakup = new tblAirBookingCostBreakup()
                        {
                            BookingCostID = Convert.ToInt64(bookingCostID),
                            BaseNet = Convert.ToDecimal(baseNet),
                            TaxNet = Convert.ToDecimal(taxNet),
                            TotalNet = Convert.ToDecimal(totalNet),
                            NetCurrency = netCurrency,
                            MarkupTypeID = markupTypeID,
                            MarkupValue = Convert.ToDouble(markupValue),
                            MarkupCurrency = markupCurrency,
                            SellAmount = Convert.ToDecimal(sellAmount),
                            SellCurrency = sellCurrency,
                            AdditionalServiceFee = Convert.ToDecimal(additionalServiceFee),
                            PaxType = PaxType,
                            NoOfPAx = NoOfPAx
                        };
                        _ctx.tblAirBookingCostBreakup.Add(_tblAirBookingCostBreakup);
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        //tblPayment
        public async Task InsertIntotblPayment(string BookingRefID, double paidAmount, string currencyCode, string paidDate, int paymentTypeID)
        {
            try
            {
                string paiddateInformat = Convert.ToDateTime(paidDate).ToString("dd/MM/yyyy");
                using (var _ctx = new MediationEntities())
                {
                    tblPayment _tblPayment = new tblPayment()
                    {
                        BookingRefID = Convert.ToInt64(BookingRefID),
                        PaidAmount = Convert.ToDecimal(paidAmount),
                        CurrencyCode = currencyCode,
                        PaidDate = Convert.ToDateTime(paiddateInformat),
                        PaymentTypeID = paymentTypeID
                    };
                    _ctx.tblPayment.Add(_tblPayment);
                    _ctx.SaveChanges();
                }

            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        public async Task InsertIntotblAirOriginDestinationOptions(string BookingRefID, List<Flleggroup> fLLegGroup)
        {
            try
            {
                int totalleg = fLLegGroup.Count;
                for (int i = 0; i < totalleg; i++)
                {
                    int refNumber = 0;
                    int directionID = i;
                    string elapsedTime = fLLegGroup[i].ElapsedTime;
                    using (var _ctx = new MediationEntities())
                    {
                        tblAirOriginDestinationOption _tblAirOriginDestinationOption = new tblAirOriginDestinationOption()
                        {
                            BookingRefID = Convert.ToInt64(BookingRefID),
                            RefNumber = refNumber,
                            DirectionID = directionID,
                            ElapsedTime = elapsedTime
                        };
                        _ctx.tblAirOriginDestinationOption.Add(_tblAirOriginDestinationOption);
                        _ctx.SaveChanges();
                        int totalsegment = fLLegGroup[i].Segments.Length;
                        for (int j = 0; j < totalsegment; j++)
                        {
                            await InsertIntotblAirSegment(BookingRefID, _tblAirOriginDestinationOption.OriginDestinationID.ToString(), fLLegGroup[i].Segments[j]);
                        }

                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        //tblAirSegment
        public async Task InsertIntotblAirSegment(string BookingRefID, string origindestinationid, Segment segments)
        {
            try
            {
                string departureDateTimeAppHH = segments.DepartureTime.Substring(0, 2);
                string departureDateTimeAppMM = segments.DepartureTime.Substring(2, 2);
                string departureDateTimeApp = segments.DepartureDate.Replace('-', '/') + " " + departureDateTimeAppHH + ":" + departureDateTimeAppMM + ":00";
                DateTime departureDateTime = DateTime.ParseExact(departureDateTimeApp, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                string arrDateTimeAppHH = segments.ArrivalTime.Substring(0, 2);
                string arrDateTimeAppMM = segments.ArrivalTime.Substring(2, 2);
                string arrDateTimeAppNew = segments.ArrivalDate.Replace('-', '/') + " " + arrDateTimeAppHH + ":" + arrDateTimeAppMM + ":00";
                DateTime arrDateTime = DateTime.ParseExact(arrDateTimeAppNew, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


                string flightNumber = segments.FlightNumber;
                string departureAirportLocationCode = segments.DepartureFrom;
                string departureAirportTerminal = segments.TerminalFrom;
                string arrivalAirportLocationCode = segments.DepartureTo;
                string arrivalAirportTerminal = segments.TerminalTo;
                string operatingAirlineCode = segments.OperatingCompany;
                string equipmentAirEquipType = segments.Flightequip;
                string marketingAirlineCode = segments.MarketingCompany;
                using (var _ctx = new MediationEntities())
                {
                    tblAirSegment _tblAirSegment = new tblAirSegment()
                    {
                        OriginDestinationID = Convert.ToInt64(origindestinationid),
                        DepartureDateTime = Convert.ToDateTime(departureDateTime),
                        ArrivalDateTime = Convert.ToDateTime(arrDateTime),
                        FlightNumber = flightNumber,
                        Status = 1,
                        DepartureAirportLocationCode = departureAirportLocationCode,
                        DepartureAirportTerminal = departureAirportTerminal,
                        ArrivalAirportLocationCode = arrivalAirportLocationCode,
                        ArrivalAirportTerminal = arrivalAirportTerminal,
                        OperatingAirlineCode = operatingAirlineCode,
                        EquipmentAirEquipType = equipmentAirEquipType,
                        MarketingAirlineCode = marketingAirlineCode
                    };
                    _ctx.tblAirSegment.Add(_tblAirSegment);
                    _ctx.SaveChanges();

                    Int64 segid = Convert.ToInt64(_tblAirSegment.SegmentID);
                    tblAirSegmentBookingAvail _tblAirSegmentBookingAvail = new tblAirSegmentBookingAvail()
                    {
                        SegmentID = Convert.ToInt64(segid),
                        ResBookDesigCode = "",
                        AvailablePTC = "",
                        ResBookDesigCabinCode = segments.BookingClass,
                        FareBasis = segments.FareBasis
                    };
                    _ctx._tblAirSegmentBookingAvail.Add(_tblAirSegmentBookingAvail);
                    _ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
        }
        //tblAirbaggageDetails
        public async Task InsertIntotblAirbaggageDetails(string BookingRefID, List<AirBagDetails> airBagDetails)
        {
            try
            {
                int totalbagdetails = airBagDetails.Count;
                for (int i = 0; i < totalbagdetails; i++)
                {
                    string paxtype = airBagDetails[i].PAXTyp;
                    int cabinBaggageQuantity = Convert.ToInt16(airBagDetails[i].CabinBaggageQuantity);
                    string cabinBaggageUnit = airBagDetails[i].CabinBaggageUnit;
                    int checkinBaggageQuantity = Convert.ToInt16(airBagDetails[i].CheckinBaggageQuantity);
                    string checkinBaggageUnit = airBagDetails[i].CheckinBaggageUnit;
                    string fromseg = airBagDetails[i].fromSeg;
                    string toseg = airBagDetails[i].toseg;
                    using (var _ctx = new MediationEntities())
                    {
                        tblAirbaggageDetails _tblAirbaggageDetails = new tblAirbaggageDetails()
                        {
                            BookingRefID = Convert.ToInt64(BookingRefID),
                            PAXType = paxtype,
                            CabinBaggageQuantity = cabinBaggageQuantity,
                            CabinBaggageUnit = cabinBaggageUnit,
                            CheckinBaggageQuantity = checkinBaggageQuantity,
                            CheckinBaggageUnit = checkinBaggageUnit,
                            FromSeg = fromseg,
                            ToSeg = toseg
                        };
                        _ctx.tblAirbaggageDetails.Add(_tblAirbaggageDetails);
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        //tblAirFarerules
        public async Task InsertIntotblAirFarerules(string BookingRefID, List<Fareruleseg> fareruleseg)
        {
            try
            {
                int totalfarerule = fareruleseg.Count;
                for (int i = 0; i < totalfarerule; i++)
                {
                    string fareRule = fareruleseg[i].FareRule;
                    string segment = fareruleseg[i].Segment;
                    string fareRef = fareruleseg[i].FareRef;
                    string filingAirline = fareruleseg[i].FilingAirline;
                    string marketingAirline = fareruleseg[i].MarketingAirline;
                    using (var _ctx = new MediationEntities())
                    {
                        tblAirFarerules _tblAirFarerules = new tblAirFarerules()
                        {
                            BookingRefID = Convert.ToInt64(BookingRefID),
                            FareRule = fareRule,
                            Segment = segment,
                            FareRef = fareRef,
                            FilingAirline = filingAirline,
                            MarketingAirline = marketingAirline
                        };
                        _ctx.tblAirFarerules.Add(_tblAirFarerules);
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        //Update tblbooking 
        public async Task UpdateTblBooking(string bookingRefID, string pnrNo)
        {
            try
            {

                using (var _ctx = new MediationEntities())
                {
                    tblBooking _tblBooking = new tblBooking()
                    {
                        BookingRefID = Convert.ToInt64(bookingRefID),
                        SupplierBookingReference = pnrNo,
                        BookingStatusCode = "HK"
                    };
                    _ctx.tblBookings.Attach(_tblBooking);
                    _ctx.Entry(_tblBooking).Property(x => x.SupplierBookingReference).IsModified = true;
                    _ctx.Entry(_tblBooking).Property(x => x.BookingStatusCode).IsModified = true;
                    _ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        public async Task UpdateTblAirpassemgers(string bookingRefID, string pnrNo)
        {
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    long _bookref = Convert.ToInt64(bookingRefID);
                    var supplierBasicDetails = (from sm in _ctx.tblAirPassengers
                                                where sm.BookingRefID == _bookref
                                                select new
                                                {
                                                    PaxID = sm.PaxID
                                                }).ToList()
                                                .Select(x => new tblAirPassengers()
                                                {
                                                    PaxID = x.PaxID
                                                }).ToList();

                    int totalpaxpaengers = supplierBasicDetails.Count;
                    for (int i = 0; i < totalpaxpaengers; i++)
                    {
                        tblAirPassengers _tblAirPassenger = new tblAirPassengers()
                        {
                            PaxID = supplierBasicDetails[i].PaxID,
                            PNR = pnrNo
                        };
                        _ctx.tblAirPassengers.Attach(_tblAirPassenger);
                        _ctx.Entry(_tblAirPassenger).Property(x => x.PNR).IsModified = true;
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        //Add Booking history
        public async Task InsertIntotblBookingHistory(string BookingRefID, string userID, string bookingStatusCode)
        {
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    tblBookingHistory _tblBookingHistory = new tblBookingHistory()
                    {
                        BookingRefID = Convert.ToInt64(BookingRefID),
                        BookingStatusCode = bookingStatusCode,
                        UserID = Convert.ToInt64(userID),
                        ActionDate = DateTime.Now
                    };
                    _ctx.tblBookingHistorys.Add(_tblBookingHistory);
                    _ctx.SaveChanges();
                }

            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        public async Task UpdateTblAirpassemgersafterIssuedTicket(string bookingRefID, string pnrNo, Itineraryinformation[] itineraryInformation)
        {
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    long _bookref = Convert.ToInt64(bookingRefID);
                    var supplierBasicDetails = (from sm in _ctx.tblAirPassengers
                                                where sm.BookingRefID == _bookref
                                                select new
                                                {
                                                    PaxID = sm.PaxID
                                                }).ToList()
                                                .Select(x => new tblAirPassengers()
                                                {
                                                    PaxID = x.PaxID
                                                }).ToList();

                    int totalpaxpaengers = supplierBasicDetails.Count;
                    for (int i = 0; i < totalpaxpaengers; i++)
                    {
                        tblAirPassengers _tblAirPassenger = new tblAirPassengers()
                        {
                            PaxID = supplierBasicDetails[i].PaxID,
                            PNR = pnrNo,
                            ETicketNo = itineraryInformation[i].ETickets.eTicketNumber
                        };
                        _ctx.tblAirPassengers.Attach(_tblAirPassenger);
                        _ctx.Entry(_tblAirPassenger).Property(x => x.PNR).IsModified = true;
                        _ctx.Entry(_tblAirPassenger).Property(x => x.ETicketNo).IsModified = true;
                        _ctx.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        public async Task<GetTripDetailsModelRS> GetTravellerDetailsfromDB(string bookingRefID)
        {
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    long _bookref = Convert.ToInt64(bookingRefID);
                    var BookingRefIDBasicData = (from sm in _ctx.tblBookings
                                                 join bs in _ctx.tblBookingStatus on sm.BookingStatusCode equals bs.BookingStatusCode
                                                 join ur in _ctx.tblUsers on sm.UserID equals ur.UserID
                                                 join ag in _ctx.tblAgencies on ur.AgencyID equals ag.AgencyID
                                                 join bc in _ctx.tblAirBookingCost on sm.BookingRefID equals bc.BookingRefID
                                                 where sm.BookingRefID == _bookref
                                                 select new
                                                 {
                                                     BookingDate = sm.BookingDate,
                                                     UserID = sm.UserID,
                                                     BookingID = sm.BookingRefID,
                                                     SupplierBookingReference = sm.SupplierBookingReference,
                                                     CancellationDeadline = sm.CancellationDeadline,
                                                     BookingStatusCode = sm.BookingStatusCode,
                                                     BookingStatus = bs.BookingStatus,
                                                     AgencyCode = ag.AgencyCode,
                                                     TotalBaseNet = bc.TotalBaseNet,
                                                     TotalTaxNet = bc.TotalTaxNet,
                                                     TotalNet = bc.TotalNet,
                                                     NetCurrency = bc.NetCurrency,
                                                     MarkupTypeID = bc.MarkupTypeID,
                                                     MarkupValue = bc.MarkupValue,
                                                     MarkupCurrency = bc.MarkupCurrency,
                                                     SellAmount = bc.SellAmount,
                                                     SellCurrency = bc.SellCurrency,
                                                     AdditionalServiceFee = bc.AdditionalServiceFee,
                                                     CancellationAmount = bc.CancellationAmount,
                                                     CancellationCurrency = bc.CancellationCurrency,
                                                     SupplierCode = ""
                                                 }).ToList();
                    //To Response Json Start
                    Dbconnectionresponse _Dbconnectionresponse = new Dbconnectionresponse()
                    {
                        AgencyCode = BookingRefIDBasicData[0].AgencyCode,
                        BookingId = BookingRefIDBasicData[0].BookingID.ToString(),
                        SupplierCode = BookingRefIDBasicData[0].SupplierCode,

                    };
                    Dbtotalfaregroup _Dbtotalfaregroup = new Dbtotalfaregroup()
                    {
                        totalBaseNet = BookingRefIDBasicData[0].TotalBaseNet.ToString(),
                        totalTaxNet = BookingRefIDBasicData[0].TotalTaxNet.ToString(),
                        markupTypeID = BookingRefIDBasicData[0].MarkupTypeID.ToString(),
                        netCurrency = BookingRefIDBasicData[0].NetCurrency.ToString(),
                        markupValue = BookingRefIDBasicData[0].MarkupValue.ToString(),
                        markupCurrency = BookingRefIDBasicData[0].MarkupCurrency,
                        sellAmount = BookingRefIDBasicData[0].SellAmount.ToString(),
                        sellCurrency = BookingRefIDBasicData[0].SellCurrency.ToString(),
                        additionalServiceFee = BookingRefIDBasicData[0].AdditionalServiceFee.ToString(),
                        cancellationAmount = BookingRefIDBasicData[0].CancellationAmount.ToString(),
                        cancellationCurrency = BookingRefIDBasicData[0].CancellationCurrency,
                        paidDate = "",
                        paidAmount = BookingRefIDBasicData[0].SellAmount.ToString(),
                        paymentTypeID = ""
                    };
                    _Dbconnectionresponse.DBTotalfaregroup = _Dbtotalfaregroup;
                    //To Response Json End
                    var RefIDBaggDetails = (from bg in _ctx.tblAirbaggageDetails
                                            where bg.BookingRefID == _bookref
                                            select new
                                            {
                                                paxType = bg.PAXType,
                                                cabinBaggageQuantity = bg.CabinBaggageQuantity,
                                                cabinBaggageUnit = bg.CabinBaggageUnit,
                                                checkinBaggageQuantity = bg.CheckinBaggageQuantity,
                                                checkinBaggageUnit = bg.CheckinBaggageUnit
                                            }).ToList();
                    //To Response Json Start
                    int totalbaggdetails = RefIDBaggDetails.Count;
                    if (totalbaggdetails > 0)
                    {
                        Dbairbagdetail[] _Dbairbagdetails = new Dbairbagdetail[totalbaggdetails];
                        for (int i = 0; i < totalbaggdetails; i++)
                        {
                            Dbairbagdetail _Dbairbagdetail = new Dbairbagdetail()
                            {
                                paxType = RefIDBaggDetails[i].paxType,
                                cabinBaggageQuantity = RefIDBaggDetails[i].cabinBaggageQuantity.ToString(),
                                cabinBaggageUnit = RefIDBaggDetails[i].cabinBaggageUnit,
                                checkinBaggageQuantity = RefIDBaggDetails[i].checkinBaggageQuantity.ToString(),
                                checkinBaggageUnit = RefIDBaggDetails[i].checkinBaggageUnit
                            };
                            _Dbairbagdetails[i] = _Dbairbagdetail;
                        }
                        _Dbconnectionresponse.DBairBagDetails = _Dbairbagdetails;
                    }
                    else
                    {
                        Dbairbagdetail[] _Dbairbagdetails = new Dbairbagdetail[0];
                        _Dbconnectionresponse.DBairBagDetails = _Dbairbagdetails;
                    }
                    //To Response Json End                    
                    var RefIDCostbreakup = (from br in _ctx.tblAirBookingCostBreakup
                                            join bc in _ctx.tblAirBookingCost on br.BookingCostID equals bc.BookingCostID
                                            where bc.BookingRefID == _bookref
                                            select new
                                            {
                                                paxType = br.PaxType,
                                                paxtotalTaxNet = br.TaxNet,
                                                paxtotalBaseNet = br.BaseNet,
                                                paxmarkupTypeID = br.MarkupTypeID,
                                                paxnetCurrency = br.NetCurrency,
                                                paxmarkupValue = br.MarkupValue,
                                                paxmarkupCurrency = br.MarkupCurrency,
                                                paxsellAmount = br.SellAmount,
                                                paxsellCurrency = br.SellCurrency,
                                                paxadditionalServiceFee = br.AdditionalServiceFee,
                                                paxquantity = br.NoOfPAx,
                                                paxpaidAmount = br.SellAmount
                                            }).ToList();
                    //To Response Json Start
                    int totalpricebk = RefIDCostbreakup.Count;
                    if (totalpricebk > 0)
                    {
                        Dbcostbreakuppax[] _Dbcostbreakuppaxs = new Dbcostbreakuppax[totalpricebk];
                        for (int i = 0; i < totalpricebk; i++)
                        {
                            Dbcostbreakuppax _Dbcostbreakuppax = new Dbcostbreakuppax()
                            {
                                paxType = RefIDCostbreakup[i].paxType,
                                paxquantity = RefIDCostbreakup[i].paxquantity.ToString(),
                                paxtotalBaseNet = RefIDCostbreakup[i].paxtotalBaseNet.ToString(),
                                paxtotalTaxNet = RefIDCostbreakup[i].paxtotalTaxNet.ToString(),
                                paxmarkupTypeID = RefIDCostbreakup[i].paxmarkupTypeID.ToString(),
                                paxnetCurrency = RefIDCostbreakup[i].paxnetCurrency.ToString(),
                                paxmarkupValue = RefIDCostbreakup[i].paxmarkupValue.ToString(),
                                paxmarkupCurrency = RefIDCostbreakup[i].paxmarkupCurrency.ToString(),
                                paxsellAmount = RefIDCostbreakup[i].paxsellAmount.ToString(),
                                paxsellCurrency = RefIDCostbreakup[i].paxsellCurrency.ToString(),
                                paxadditionalServiceFee = RefIDCostbreakup[i].paxadditionalServiceFee.ToString(),
                                paxpaidAmount = RefIDCostbreakup[i].paxpaidAmount.ToString()
                            };
                            _Dbcostbreakuppaxs[i] = _Dbcostbreakuppax;
                        }
                        _Dbconnectionresponse.DBcostBreakuppax = _Dbcostbreakuppaxs;
                    }
                    else
                    {
                        Dbcostbreakuppax[] _Dbcostbreakuppaxs = new Dbcostbreakuppax[0];
                        _Dbconnectionresponse.DBcostBreakuppax = _Dbcostbreakuppaxs;
                    }
                    //To Response Json End                    
                    var RefIDfarerule = (from fr in _ctx.tblAirFarerules
                                         where fr.BookingRefID == _bookref
                                         select new
                                         {
                                             FareRule = fr.FareRule,
                                             Segment = fr.Segment,
                                             FareRef = fr.FareRef,
                                             FilingAirline = fr.FilingAirline,
                                             MarketingAirline = fr.MarketingAirline
                                         }).ToList();
                    //To Response Json Start
                    int totRefIDfarerule = RefIDfarerule.Count;
                    if (totRefIDfarerule > 0)
                    {
                        Dbfareruleseg[] _Dbfarerulesegs = new Dbfareruleseg[totRefIDfarerule];
                        for (int i = 0; i < totRefIDfarerule; i++)
                        {
                            Dbfareruleseg _Dbfareruleseg = new Dbfareruleseg()
                            {
                                fareRule = RefIDfarerule[i].FareRule,
                                Segment = RefIDfarerule[i].Segment,
                                FareRef = RefIDfarerule[i].FareRef,
                                FilingAirline = RefIDfarerule[i].FilingAirline,
                                MarketingAirline = RefIDfarerule[i].MarketingAirline
                            };
                            _Dbfarerulesegs[i] = _Dbfareruleseg;
                        }
                        _Dbconnectionresponse.DBfareruleseg = _Dbfarerulesegs;
                    }
                    else
                    {
                        Dbfareruleseg[] _Dbfarerulesegs = new Dbfareruleseg[0];
                        _Dbconnectionresponse.DBfareruleseg = _Dbfarerulesegs;
                    }
                    //To Response Json End                    
                    var RefIDTravellers = (from ap in _ctx.tblAirPassengers
                                           join tl in _ctx.tblTitles on ap.TitleID equals tl.TitleID
                                           join pd in _ctx._tblAirPassengerDoc on ap.PaxID equals pd.PaxID
                                           join doc in _ctx._tblDocType on pd.DocTypeID equals doc.DocTypeID
                                           where ap.BookingRefID == _bookref
                                           select new
                                           {
                                               Title = tl.Title,
                                               GivenName = ap.GivenName,
                                               Surname = ap.Surname,
                                               BirthDate = ap.DateofBirth,
                                               DocType = doc.DocType,
                                               DocumentNumber = pd.DocumentNumber,
                                               DocIssueCountry = pd.IssueCountry,
                                               ExpireDate = pd.ExpiryDate,
                                               AirPnr = ap.PNR,
                                               AirTicketNo = ap.ETicketNo,
                                               paxtype = ap.PaxTypeID
                                           }).ToList();
                    //To Response Json Start
                    int totRefIDTravellers = RefIDTravellers.Count;
                    if (totRefIDTravellers > 0)
                    {
                        Dbtravelerinfo[] _Dbtravelerinfos = new Dbtravelerinfo[totRefIDTravellers];
                        for (int i = 0; i < totRefIDTravellers; i++)
                        {
                            Dbtravelerinfo _Dbtravelerinfo = new Dbtravelerinfo()
                            {
                                PassengerType = RefIDTravellers[i].paxtype.ToString(),
                                GivenName = RefIDTravellers[i].GivenName,
                                NamePrefix = RefIDTravellers[i].Title,
                                Surname = RefIDTravellers[i].Surname,
                                BirthDate = Convert.ToDateTime(RefIDTravellers[i].BirthDate).ToString("dd-MM-yyyy"),
                                DocType = RefIDTravellers[i].DocType,
                                DocumentNumber = RefIDTravellers[i].DocumentNumber,
                                DocIssueCountry = RefIDTravellers[i].DocIssueCountry,
                                ExpireDate = Convert.ToDateTime(RefIDTravellers[i].ExpireDate).ToString("dd-MM-yyyy"),
                                AirPnr = RefIDTravellers[i].AirPnr,
                                AirTicketNo = RefIDTravellers[i].AirTicketNo
                            };
                            _Dbtravelerinfos[i] = _Dbtravelerinfo;
                        }
                        _Dbconnectionresponse.DBTravelerInfo = _Dbtravelerinfos;
                    }
                    else
                    {
                        Dbtravelerinfo[] _Dbtravelerinfos = new Dbtravelerinfo[0];
                        _Dbconnectionresponse.DBTravelerInfo = _Dbtravelerinfos;
                    }
                    //To Response Json End                    
                    var RefiDLegs = (from lg in _ctx.tblAirOriginDestinationOption
                                     where lg.BookingRefID == _bookref
                                     select new
                                     {
                                         OriginDestinationID = lg.OriginDestinationID,
                                         RefNumber = lg.RefNumber,
                                         DirectionID = lg.RefNumber,
                                         ElapsedTime = lg.ElapsedTime
                                     }).ToList();
                    int totRefiDLegs = RefiDLegs.Count;
                    if (totRefiDLegs > 0)
                    {
                        Dbflleggroup[] _Dbflleggroups = new Dbflleggroup[totRefiDLegs];
                        for (int i = 0; i < totRefiDLegs; i++)
                        {
                            Dbflleggroup _Dbflleggroup = new Dbflleggroup();
                            _Dbflleggroup.elapsedtime = RefiDLegs[i].ElapsedTime;
                            long _LegID = RefiDLegs[i].OriginDestinationID;
                            var RefIDSegmant = (from seg in _ctx.tblAirSegment
                                                join segf in _ctx._tblAirSegmentBookingAvail on seg.SegmentID equals segf.SegmentID
                                                where seg.OriginDestinationID == _LegID
                                                select new
                                                {
                                                    FareBasis = "",
                                                    DepartureDate = seg.DepartureDateTime,
                                                    ArrivalDate = seg.ArrivalDateTime,
                                                    DepartureFrom = seg.DepartureAirportLocationCode,
                                                    DepartureTo = seg.ArrivalAirportLocationCode,
                                                    MarketingCompany = seg.MarketingAirlineCode,
                                                    OperatingCompany = seg.OperatingAirlineCode,
                                                    FlightNumber = seg.FlightNumber,
                                                    BookingClass = "",
                                                    terminalTo = seg.ArrivalAirportTerminal,
                                                    terminalFrom = seg.DepartureAirportTerminal,
                                                    flightequip = seg.EquipmentAirEquipType,
                                                    cabin = segf.ResBookDesigCabinCode
                                                }).ToList();
                            int totRefIDSegmant = RefIDSegmant.Count;
                            if (totRefIDSegmant > 0)
                            {
                                Dbsegment[] _Dbsegments = new Dbsegment[totRefIDSegmant];
                                for (int j = 0; j < totRefIDSegmant; j++)
                                {
                                    Dbsegment _Dbsegment = new Dbsegment();
                                    _Dbsegment.DepartureDate = Convert.ToDateTime(RefIDSegmant[j].DepartureDate).ToString("dd-MM-yyyy");
                                    _Dbsegment.DepartureTime = Convert.ToDateTime(RefIDSegmant[j].DepartureDate).ToString("hhmm");
                                    _Dbsegment.ArrivalDate = Convert.ToDateTime(RefIDSegmant[j].ArrivalDate).ToString("dd-MM-yyyy");
                                    _Dbsegment.ArrivalTime = Convert.ToDateTime(RefIDSegmant[j].ArrivalDate).ToString("hhmm");
                                    _Dbsegment.DepartureFrom = RefIDSegmant[j].DepartureFrom;
                                    _Dbsegment.DepartureTo = RefIDSegmant[j].DepartureTo;
                                    _Dbsegment.MarketingCompany = RefIDSegmant[j].MarketingCompany;
                                    _Dbsegment.OperatingCompany = RefIDSegmant[j].OperatingCompany;
                                    _Dbsegment.FlightNumber = RefIDSegmant[j].FlightNumber;
                                    _Dbsegment.BookingClass = RefIDSegmant[j].BookingClass;
                                    _Dbsegment.terminalTo = RefIDSegmant[j].terminalTo;
                                    _Dbsegment.terminalFrom = RefIDSegmant[j].terminalFrom;
                                    _Dbsegment.flightequip = RefIDSegmant[j].flightequip;
                                    _Dbsegment.cabin = RefIDSegmant[j].cabin;
                                    _Dbsegments[j] = _Dbsegment;
                                }
                                _Dbflleggroup.from = RefIDSegmant[0].DepartureFrom;
                                _Dbflleggroup.to = RefIDSegmant[totRefIDSegmant - 1].DepartureTo;
                                _Dbflleggroup.DBsegments = _Dbsegments;
                            }
                            else
                            {
                                Dbsegment[] _Dbsegments = new Dbsegment[0];
                                _Dbflleggroup.DBsegments = _Dbsegments;
                            }
                            _Dbflleggroups[i] = _Dbflleggroup;
                        }
                        _Dbconnectionresponse.DBFLLegGroup = _Dbflleggroups;
                    }
                    else
                    {
                        Dbflleggroup[] _Dbflleggroups = new Dbflleggroup[0];
                        _Dbconnectionresponse.DBFLLegGroup = _Dbflleggroups;
                    }
                    var refidPaxAdtcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "ADT"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx

                                            }).ToList();
                    var refidPaxchdcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "CHD"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx

                                            }).ToList();
                    var refidPaxinfcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "INF"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx

                                            }).ToList();
                    if (refidPaxAdtcound.Count > 0)
                    {
                        _Dbconnectionresponse.ADT = Convert.ToInt16(refidPaxAdtcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.ADT = 0;
                    }
                    if (refidPaxchdcound.Count > 0)
                    {
                        _Dbconnectionresponse.CHD = Convert.ToInt16(refidPaxchdcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.CHD = 0;
                    }
                    if (refidPaxinfcound.Count > 0)
                    {
                        _Dbconnectionresponse.INF = Convert.ToInt16(refidPaxinfcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.INF = 0;
                    }
                    _GetTripDetailsModelRS.DBConnectionResponse = _Dbconnectionresponse;
                }
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
            return _GetTripDetailsModelRS;
        }
        public async Task InsertIntotblBookingData(string BookingRefID, string PNR, string uniqueID)
        {
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    tblAirBookingData _tblAirBookingData = new tblAirBookingData()
                    {
                        BookingRefID = Convert.ToInt64(BookingRefID),
                        PNR = PNR,
                        UniqueID = uniqueID
                    };
                    _ctx._tblAirBookingData.Add(_tblAirBookingData);
                    _ctx.SaveChanges();
                }

            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

            }
        }
        public async Task<string> GetUserIDfromRefID(string BookingRefID)
        {
            string userID = "";
            long _bookrefid = Convert.ToInt64(BookingRefID);
            using (var _ctx = new MediationEntities())
            {
                var refIDuser = (from us in _ctx.tblBookings
                                 where us.BookingRefID == _bookrefid
                                 select new
                                 {
                                     UserID = us.UserID
                                 }).ToList();
                userID = refIDuser[0].UserID.ToString();
            }
            return userID;
        }
    }
}
