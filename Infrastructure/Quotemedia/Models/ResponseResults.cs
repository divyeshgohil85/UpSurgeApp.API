using System;
using System.Collections.Generic;

namespace Infrastructure.Quotemedia.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Key
    {
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string exLgName { get; set; }
        public string exShName { get; set; }
    }

    public class Equityinfo
    {
        public string longname { get; set; }
        public string shortname { get; set; }
    }

    public class Status
    {
    }

    public class Pricedata
    {
        public double last { get; set; }
        public double change { get; set; }
        public double changepercent { get; set; }
        public int tick { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double prevclose { get; set; }
        public double bid { get; set; }
        public double ask { get; set; }
        public int bidsize { get; set; }
        public int asksize { get; set; }
        public int rawbidsize { get; set; }
        public int rawasksize { get; set; }
        public int tradevolume { get; set; }
        public int sharevolume { get; set; }
        public double vwap { get; set; }
        public DateTime lasttradedatetime { get; set; }
        public DateTime lastquotedatetime { get; set; }
        public double totalvalue { get; set; }
    }

    public class Week52high
    {
        public string date { get; set; }
        public double content { get; set; }
    }

    public class Week52low
    {
        public string date { get; set; }
        public double content { get; set; }
    }

    public class Latestamount
    {
        public string currency { get; set; }
        public double content { get; set; }
    }

    public class Dividend
    {
        public string date { get; set; }
        public double amount { get; set; }
        public double yield { get; set; }
        public Latestamount latestamount { get; set; }
        public string frequency { get; set; }
        public string paydate { get; set; }
    }

    public class Fundamental
    {
        public long sharesoutstanding { get; set; }
        public long shareclasslevelsharesoutstanding { get; set; }
        public long totalsharesoutstanding { get; set; }
        public object marketcap { get; set; }
        public double? eps { get; set; }
        public double? pbratio { get; set; }
        public Week52high week52high { get; set; }
        public Week52low week52low { get; set; }
        public double? peratio { get; set; }
        public Dividend dividend { get; set; }
        public int? sharesescrow { get; set; }
    }

    public class Quote
    {
        public Key key { get; set; }
        public Equityinfo equityinfo { get; set; }
        public Status status { get; set; }
        public Pricedata pricedata { get; set; }
        public Fundamental fundamental { get; set; }
        public string symbolstring { get; set; }
        public string datatype { get; set; }
        public string entitlement { get; set; }
        public int? delaymin { get; set; }
        public DateTime datetime { get; set; }
    }

    public class IndustryComparison
    {
        public int rankCompany { get; set; }
        public int numCompIndustry { get; set; }
        public string industry { get; set; }
    }

    public class StrongSell
    {
        public int month1 { get; set; }
        public int current { get; set; }
        public int month2 { get; set; }
        public int month3 { get; set; }
    }

    public class MeanRecommend
    {
        public double month1 { get; set; }
        public double current { get; set; }
        public double month2 { get; set; }
        public double month3 { get; set; }
        public double change { get; set; }
        public int numReporting { get; set; }
    }

    public class StrongBuy
    {
        public int month1 { get; set; }
        public int current { get; set; }
        public int month2 { get; set; }
        public int month3 { get; set; }
    }

    public class ModerateBuy
    {
        public int month1 { get; set; }
        public int current { get; set; }
        public int month2 { get; set; }
        public int month3 { get; set; }
    }

    public class ModerateSell
    {
        public int month1 { get; set; }
        public int current { get; set; }
        public int month2 { get; set; }
        public int month3 { get; set; }
    }

    public class Hold
    {
        public int month1 { get; set; }
        public int current { get; set; }
        public int month2 { get; set; }
        public int month3 { get; set; }
    }

    public class Analyst
    {
        public IndustryComparison industryComparison { get; set; }
        public StrongSell strongSell { get; set; }
        public MeanRecommend meanRecommend { get; set; }
        public StrongBuy strongBuy { get; set; }
        public ModerateBuy moderateBuy { get; set; }
        public ModerateSell moderateSell { get; set; }
        public string symbolstring { get; set; }
        public Hold hold { get; set; }
    }

    public class Lookupdata
    {
        public Equityinfo equityinfo { get; set; }
        public Key key { get; set; }
        public string symbolstring { get; set; }
    }

    public class Symbolinfo
    {
        public Equityinfo equityinfo { get; set; }
        public Key key { get; set; }
        public string symbolstring { get; set; }
    }

    public class Annualinfo
    {
        public int latestfiscaldividendspershare { get; set; }
        public string latestfiscaldate { get; set; }
        public int latestfiscalrevenue { get; set; }
        public string latestfiscalEPS { get; set; }
    }

    public class Priceinfo
    {
        public double day21movingavg { get; set; }
        public double day50movingavg { get; set; }
        public double r2 { get; set; }
        public double weeks52high { get; set; }
        public double day50ema { get; set; }
        public double weeks52low { get; set; }
        public double day200ema { get; set; }
        public double alpha { get; set; }
        public double day200movingavg { get; set; }
        public string weeks52change { get; set; }
        public int periods { get; set; }
        public double day21ema { get; set; }
        public double stddev { get; set; }
        public double beta { get; set; }
    }

    public class Shareinformation
    {
        public int outstanding { get; set; }
        public int shareclasslevelsharesoutstanding { get; set; }
        public double shareturnover1year { get; set; }
        public string shortint { get; set; }
        public string eps { get; set; }
        public int avgvol50days { get; set; }
        public int avgvol20days { get; set; }
        public int @float { get; set; }
        public string shortintratio { get; set; }
        public string shortdate { get; set; }
        public int avgvol10days { get; set; }
        public int pctfloat { get; set; }
        public string shortpctflt { get; set; }
        public int avgvol30days { get; set; }
        public int epsvar10year { get; set; }
    }

    public class Holdings
    {
        public string instituteholdingsdate { get; set; }
        public string institutions { get; set; }
        public int insidersoldprev3mos { get; set; }
        public string instituteboughtprev3mos { get; set; }
        public int insiderboughtprev3mos { get; set; }
        public string instituteholdingspct { get; set; }
        public string totalheld { get; set; }
        public string insiderholdingspct { get; set; }
        public string insiderholdingsdate { get; set; }
        public string institutesoldprev3mos { get; set; }
        public string insidersharesowned { get; set; }
    }

    public class Pricechange
    {
        public double day21pct { get; set; }
        public double monthtodate { get; set; }
        public string day200 { get; set; }
        public string day200pct { get; set; }
        public double day90pct { get; set; }
        public double quartertodate { get; set; }
        public double quartertodatepct { get; set; }
        public string yeartodate { get; set; }
        public double monthtodatepct { get; set; }
        public double day21 { get; set; }
        public double day7 { get; set; }
        public double day30 { get; set; }
        public string yeartodatepct { get; set; }
        public double day30pct { get; set; }
        public double day90 { get; set; }
        public double day180pct { get; set; }
        public double day180 { get; set; }
        public double day7pct { get; set; }
    }

    public class Shareinfo
    {
        public Annualinfo annualinfo { get; set; }
        public Priceinfo priceinfo { get; set; }
        public Shareinformation shareinformation { get; set; }
        public Holdings holdings { get; set; }
        public Pricechange pricechange { get; set; }
    }

    public class Index
    {
        public string indexname { get; set; }
        public string indexsymbol { get; set; }
    }

    public class Indices
    {
        public Index index { get; set; }
    }

    public class Details
    {
        public string issuetype { get; set; }
        public long? marketcap { get; set; }
        //public Indices indices { get; set; }
        public string auditor { get; set; }
        public string ceo { get; set; }
        public int? employees { get; set; }
        public string lastAudit { get; set; }
    }

    public class Sic
    {
        public string name { get; set; }
        public int content { get; set; }
    }

    public class Sics
    {
        public Sic sic { get; set; }
    }

    public class Classification
    {
        public int? qmid { get; set; }
        //public Sics sics { get; set; }
        public int? cik { get; set; }
        public string industry { get; set; }
        public string sector { get; set; }
        public string naics { get; set; }
        public string qmdescription { get; set; }
    }

    public class Address
    {
        public string country { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string address1 { get; set; }
        public string postcode { get; set; }
        public string state { get; set; }
    }

    public class Info
    {
        public string website { get; set; }
        public Address address { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
    }

    public class Profile
    {
        public string shortdescription { get; set; }
        public string longdescription { get; set; }
        public Details details { get; set; }
        public Classification classification { get; set; }
        public Info info { get; set; }
    }

    public class Profitability
    {
        public string ebitmargin { get; set; }
        public string ebitdamargin { get; set; }
        public string pretaxprofitmargin { get; set; }
        public string profitmargincont { get; set; }
        public int profitvar10year { get; set; }
        public string grossmargin { get; set; }
        public string profitmargintot { get; set; }
    }

    public class Incomestatements
    {
        public int revenue { get; set; }
        public int revenuepershare { get; set; }
        public string revenue3years { get; set; }
        public string revenue5years { get; set; }
    }

    public class Valuationmeasures
    {
        public string peratio { get; set; }
        public string enterprisevalue { get; set; }
        public string pricetosales { get; set; }
        public string pricetofreecash { get; set; }
        public string pehighlast5years { get; set; }
        public string pricetobook { get; set; }
        public string pricetocashflow { get; set; }
        public string pelowlast5years { get; set; }
        public string pricetotangiblebook { get; set; }
    }

    public class Financialstrength
    {
        public int totaldebttoequity { get; set; }
        public string intcoverage { get; set; }
        public double currentratio { get; set; }
        public double leverageratio { get; set; }
        public double quickratio { get; set; }
        public string longtermdebttocapital { get; set; }
    }

    public class Assets
    {
        public string receivablesturnover { get; set; }
        public string invoiceturnover { get; set; }
        public int assetsturnover { get; set; }
    }

    public class Managementeffectiveness
    {
        public double returnonassets { get; set; }
        public double returnonequity { get; set; }
        public double returnoncapital { get; set; }
    }

    public class Dividendssplits
    {
        public string dividend3years { get; set; }
        public string dividend5years { get; set; }
        public string exdividenddate { get; set; }
        public string dividendrate { get; set; }
        public string dividendyield { get; set; }
        public string paymenttype { get; set; }
    }

    public class Keyratios
    {
        public Profitability profitability { get; set; }
        public Incomestatements incomestatements { get; set; }
        public Valuationmeasures valuationmeasures { get; set; }
        public Financialstrength financialstrength { get; set; }
        public Assets assets { get; set; }
        public Managementeffectiveness managementeffectiveness { get; set; }
        public Dividendssplits dividendssplits { get; set; }
    }

    public class Company
    {
        public Symbolinfo symbolinfo { get; set; }
        public Profile profile { get; set; }
    }

    public class Results
    {
        public string copyright { get; set; }
        public int symbolcount { get; set; }
        public List<Quote> quote { get; set; }
        public Analyst analyst { get; set; }
        public List<Lookupdata> lookupdata { get; set; }
        public Company company { get; set; }
    }

    public class Root
    {
        public Results results { get; set; }
    }
}
