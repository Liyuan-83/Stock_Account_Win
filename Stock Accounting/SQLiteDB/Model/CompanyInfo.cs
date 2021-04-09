using MySQLiteDB.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLiteDB.Model
{
    class CompanyInfo : _DefaultModel
    {
        public static new string TABLE_NAME = "company_info";

        public enum Company_level { listed = 0, OTC = 1 }

        public string UpdateDate { get; set; }

        public new string ID { get; set; }
        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Registration { get; set; }

        public int Industry { get; set; }

        public string Address { get; set; }

        public string BusinessNumber { get; set; }

        public string Chairman { get; set; }

        public string GM { get; set; }

        public string Speaker { get; set; }

        public string SpeakerTitle { get; set; }

        public string ActingSpeaker { get; set; }

        public string Phone { get; set; }

        public string StartDate { get; set; }

        public string ListingDate { get; set; }

        public string DenominationPerShareOfCommonStock { get; set; }

        public long PaidinCapital { get; set; }

        public long NumberOfPrivateEquity { get; set; }

        public long SpecialShares { get; set; }

        public int TypesOfPreparationOfFinancialStatements { get; set; }

        public string TransferAgency { get; set; }

        public string TransferPhone { get; set; }

        public string TransferAddress { get; set; }

        public string VisaAccountingFirm { get; set; }

        public string VisaAccountant1 { get; set; }

        public string VisaAccountant2 { get; set; }

        public string EnglishAbbreviation { get; set; }

        public string EnglishCorrespondenceAddress { get; set; }

        public string FaxMachineNumber { get; set; }

        public string Email { get; set; }

        public string URL { get; set; }

        public int Level { get; set; }

        public override String TableName() => TABLE_NAME;

        public CompanyInfo() { }

        public CompanyInfo(SQLiteDataReader reader)
        {
            UpdateDate = reader["出表日期"].ToString();
            ID = reader["公司代號"].ToString();
            Name = reader["公司名稱"].ToString();
            Nickname = reader["公司簡稱"].ToString();
            Registration = reader["外國企業註冊地國"].ToString();
            Industry = Int32.Parse(reader["產業別"].ToString());
            Address = reader["住址"].ToString();
            BusinessNumber = reader["營利事業統一編號"].ToString();
            Chairman = reader["董事長"].ToString();
            GM = reader["總經理"].ToString();
            Speaker = reader["發言人"].ToString();
            SpeakerTitle = reader["發言人職稱"].ToString();
            ActingSpeaker = reader["代理發言人"].ToString();
            Phone = reader["總機電話"].ToString();
            StartDate = reader["成立日期"].ToString();
            ListingDate = reader["上市日期"].ToString();
            DenominationPerShareOfCommonStock = reader["普通股每股面額"].ToString();
            PaidinCapital = Int64.Parse(reader["實收資本額"].ToString());
            NumberOfPrivateEquity = Int64.Parse(reader["私募股數"].ToString());
            SpecialShares = Int64.Parse(reader["特別股"].ToString());
            TypesOfPreparationOfFinancialStatements = Int32.Parse(reader["編制財務報表類型"].ToString());
            TransferAgency = reader["股票過戶機構"].ToString();
            TransferPhone = reader["過戶電話"].ToString();
            TransferAddress = reader["過戶地址"].ToString();
            VisaAccountingFirm = reader["簽證會計師事務所"].ToString();
            VisaAccountant1 = reader["簽證會計師1"].ToString();
            VisaAccountant2 = reader["簽證會計師2"].ToString();
            EnglishAbbreviation = reader["英文簡稱"].ToString();
            EnglishCorrespondenceAddress = reader["英文通訊地址"].ToString();
            FaxMachineNumber = reader["傳真機號碼"].ToString();
            Email = reader["電子郵件信箱"].ToString();
            URL = reader["網址"].ToString();
            Level = Int32.Parse(reader["上市上櫃"].ToString());
        }

        public CompanyInfo(string[] strArr, Company_level level)
        {
            UpdateDate = strArr[0];
            ID = strArr[1];
            Name = strArr[2];
            Nickname = strArr[3];
            Registration = strArr[4];
            Industry = Int32.Parse(strArr[5]);
            Address = strArr[6];
            BusinessNumber = strArr[7];
            Chairman = strArr[8];
            GM = strArr[9];
            Speaker = strArr[10];
            SpeakerTitle = strArr[11];
            ActingSpeaker = strArr[12];
            Phone = strArr[13];
            StartDate = strArr[14];
            ListingDate = strArr[15];
            DenominationPerShareOfCommonStock = strArr[16];
            PaidinCapital = Int64.Parse(strArr[17]);
            NumberOfPrivateEquity = Int64.Parse(strArr[18]);
            SpecialShares = Int64.Parse(strArr[19]);
            TypesOfPreparationOfFinancialStatements = Int32.Parse(strArr[20]);
            TransferAgency = strArr[21];
            TransferPhone = strArr[22];
            TransferAddress = strArr[23];
            VisaAccountingFirm = strArr[24];
            VisaAccountant1 = strArr[25];
            VisaAccountant2 = strArr[26];
            EnglishAbbreviation = strArr[27];
            EnglishCorrespondenceAddress = strArr[28];
            FaxMachineNumber = strArr[29];
            Email = strArr[30];
            URL = strArr[31];
            Level = (int)level;
        }

        public override string CreateTable()
        {

            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (出表日期 TEXT,公司代號 TEXT PRIMARY KEY, 公司名稱 TEXT, 公司簡稱 TEXT, 外國企業註冊地國 TEXT, 產業別 INTEGER, 住址 TEXT, 營利事業統一編號 TEXT, 董事長 TEXT, 總經理 TEXT, 發言人 TEXT, 發言人職稱 TEXT, 代理發言人 assets, 總機電話 TEXT, 成立日期 TEXT, 上市日期 TEXT, 普通股每股面額 TEXT, 實收資本額 INTEGER, 私募股數 INTEGER, 特別股 INTEGER, 編制財務報表類型 INTEGER, 股票過戶機構 TEXT, 過戶電話 TEXT, 過戶地址 TEXT, 簽證會計師事務所 TEXT, 簽證會計師1 TEXT, 簽證會計師2 TEXT, 英文簡稱 TEXT, 英文通訊地址 TEXT, 傳真機號碼 TEXT, 電子郵件信箱 TEXT, 網址 TEXT, 上市上櫃 INTEGER)";
        }

        public override string InsertOrUpdateValue()
        {
            return "INSERT OR IGNORE INTO " + TABLE_NAME + " VALUES (" +
                "'" + UpdateDate + "', " +
                "'" + ID + "'," +
                "'" + Name + "'," +
                "'" + Nickname + "'," +
                "'" + Registration + "'," +
                "" + Industry + "," +
                "'" + Address + "'," +
                "'" + BusinessNumber + "'," +
                "'" + Chairman + "'," +
                "'" + GM + "'," +
                "'" + Speaker + "'," +
                "'" + SpeakerTitle + "'," +
                "'" + ActingSpeaker + "'," +
                "'" + Phone + "'," +
                "'" + StartDate + "'," +
                "'" + ListingDate + "'," +
                "'" + DenominationPerShareOfCommonStock + "'," +
                "" + PaidinCapital + "," +
                "" + NumberOfPrivateEquity + "," +
                "" + SpecialShares + "," +
                "" + TypesOfPreparationOfFinancialStatements + "," +
                "'" + TransferAgency + "'," +
                "'" + TransferPhone + "'," +
                "'" + TransferAddress + "'," +
                "'" + VisaAccountingFirm + "'," +
                "'" + VisaAccountant1 + "'," +
                "'" + VisaAccountant2 + "'," +
                "'" + EnglishAbbreviation + "'," +
                "'" + EnglishCorrespondenceAddress + "'," +
                "'" + FaxMachineNumber + "'," +
                "'" + Email + "'," +
                "'" + URL + "'," +
                "" + Level + ");" +
                "UPDATE " + TABLE_NAME + " SET " +
                "出表日期 = '" + UpdateDate + "'," +
                "公司代號 = '" + ID + "'," +
                "公司名稱 = '" + Name + "'," +
                "公司簡稱 = '" + Nickname + "'," +
                "外國企業註冊地國 = '" + Registration + "'," +
                "產業別 = " + Industry + "," +
                "住址 = '" + Address + "'," +
                "營利事業統一編號 = '" + BusinessNumber + "'," +
                "董事長 = '" + Chairman + "'," +
                "總經理 = '" + GM + "'," +
                "發言人 = '" + Speaker + "'," +
                "發言人職稱 = '" + SpeakerTitle + "'," +
                "代理發言人 = '" + ActingSpeaker + "'," +
                "總機電話 = '" + Phone + "'," +
                "成立日期 = '" + StartDate + "'," +
                "上市日期 = '" + ListingDate + "'," +
                "普通股每股面額 = '" + DenominationPerShareOfCommonStock + "'," +
                "實收資本額 = " + PaidinCapital + "," +
                "私募股數 = " + NumberOfPrivateEquity + "," +
                "特別股 = " + SpecialShares + "," +
                "編制財務報表類型 = " + TypesOfPreparationOfFinancialStatements + "," +
                "股票過戶機構 = '" + TransferAgency + "'," +
                "過戶電話 = '" + TransferPhone + "'," +
                "過戶地址 = '" + TransferAddress + "'," +
                "簽證會計師事務所 = '" + VisaAccountingFirm + "'," +
                "簽證會計師1 = '" + VisaAccountant1 + "'," +
                "簽證會計師2 = '" + VisaAccountant2 + "'," +
                "英文簡稱 = '" + EnglishAbbreviation + "'," +
                "英文通訊地址 = '" + EnglishCorrespondenceAddress + "'," +
                "傳真機號碼 = '" + FaxMachineNumber + "'," +
                "電子郵件信箱 = '" + Email + "'," +
                "網址 = '" + URL + "'," +
                "上市上櫃 = " + Level + " WHERE 公司代號 = '" + ID + "';";
        }
    }
}
