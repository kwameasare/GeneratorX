using System.ComponentModel;

namespace GeneratorXWeb.Models
{
    
        public enum DataType
        {
        [Description("int")]
        gen_int = 1,
        [Description("bool")]
        gen_bool =2,
        [Description("byte")]
        gen_byte = 3 ,
            [Description("sbyte")]
            gen_sbyte=4  ,
            [Description("char")]
            gen_char= 5 ,
            [Description("decimal")]
            gen_decimal= 6 ,
            [Description("double")]
            gen_double= 7 ,
            [Description("float")]
            gen_float= 8 ,
            [Description("dynamic")]
            gen_dynamic = 9 ,
            [Description("uint")]
            gen_uint= 10 ,
            [Description("nint")]
            gen_nint= 11 ,
            [Description("nuint")]
            gen_nuint  = 12 ,
            [Description("long")]
            gen_long = 13 ,
            [Description("short")]
            gen_short = 14 ,
            [Description("object")]
            gen_object	= 15 ,
            [Description("string")]
            gen_string	= 16 ,
          [Description("DateTime")]
            gen_date_time	= 17 ,
          


    }

    public enum ValueSource
    {
       
        Input = 1,
        OtherFeature = 2,
        LookUp = 3,
        Computed = 4,

    }

    public enum ProjectArea
    {
        [Description("Master")]
        Master = 1,
        [Description("Setup")]
        Setup = 2,
        [Description("Transaction")]
        Transaction = 3,
        [Description("Authentication")]
        Auth = 4,
        [Description("Admin")]
        Admin = 5,

    }
    public enum LookUpCodeId
    {
        Country = 1,
        IncomeType = 108,
        QuotationBasis = 109,
        ValuationBasis = 110,
        ParticipantType = 111,
        InterestType = 112,
        RollOverType = 113,
        SecurityDividend = 114,
        CorporateAction = 115,
        IssuerStatusCode = 116,
        FixedTermSecurityStatus = 117,
        RedemptionType = 118,
        AccountEntryCode = 82,
        DividendTypeCode = 119,
        ChargeFrequency = 29,
        ChargeCriteria = 30,
        ChargeCategory = 31,
        ChargeType = 32,
        none=999

    }
}

 
 
 
	
	
