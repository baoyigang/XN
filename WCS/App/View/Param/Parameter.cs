using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Util;

namespace App.View.Param
{
    public class Parameter: BaseObject
    {
        private string wcsUrl;
        [CategoryAttribute("��������Ӳ���"), DescriptionAttribute("�ܿ�WCS WebService��ַ"), Chinese("Url")]
        public string WcsUrl
        {
            get { return wcsUrl; }
            set { wcsUrl = value; }
        }

        private string warehouseCode;

        [CategoryAttribute("��������Ӳ���"), DescriptionAttribute("�ֿ����"), Chinese("�ֿ����")]
        public string WarehouseCode
        {
            get { return warehouseCode; }
            set { warehouseCode = value; }
        }

        private string sendInterval;

        [CategoryAttribute("��������Ӳ���"), DescriptionAttribute("�ϱ�ʱ��Ƶ��"), Chinese("�ϱ�ʱ��Ƶ��")]
        public string SendInterval
        {
            get { return sendInterval; }
            set { sendInterval = value; }
        }
        private string requireAPReady;

        [CategoryAttribute("��������Ӳ���"), DescriptionAttribute("�жϳ���վ̨Ready"), Chinese("�жϳ���վ̨Ready")]
        public string RequireAPReady
        {
            get { return requireAPReady; }
            set { requireAPReady = value; }
        }
        private string plc1ServerName;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC1ServerName
        {
            get { return plc1ServerName; }
            set { plc1ServerName = value; }
        }

        private string plc1ServerIp;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC1ServerIP
        {
            get { return plc1ServerIp; }
            set { plc1ServerIp = value; }
        }


        private string plc1GroupString;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("1�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC1GroupString
        {
            get { return plc1GroupString; }
            set { plc1GroupString = value; }
        }

        private int plc1UpdateRate;
        [CategoryAttribute("1�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("1�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC1UpdateRate
        {
            get { return plc1UpdateRate; }
            set { plc1UpdateRate = value; }
        }


        private string plc2ServerName;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC2ServerName
        {
            get { return plc2ServerName; }
            set { plc2ServerName = value; }
        }

        private string plc2ServerIp;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC2ServerIP
        {
            get { return plc2ServerIp; }
            set { plc2ServerIp = value; }
        }


        private string plc2GroupString;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("2�ŶѶ��ͨѶ��������"), Chinese("��������")]
        public string PLC2GroupString
        {
            get { return plc2GroupString; }
            set { plc2GroupString = value; }
        }

        private int plc2UpdateRate;
        [CategoryAttribute("2�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("2�ŶѶ��ˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC2UpdateRate
        {
            get { return plc2UpdateRate; }
            set { plc2UpdateRate = value; }
        }
        private string plc3ServerName;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC3ServerName
        {
            get { return plc3ServerName; }
            set { plc3ServerName = value; }
        }

        private string plc3ServerIp;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC3ServerIP
        {
            get { return plc3ServerIp; }
            set { plc3ServerIp = value; }
        }


        private string plc3GroupString;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("С��PLCͨѶ��������"), Chinese("��������")]
        public string PLC3GroupString
        {
            get { return plc3GroupString; }
            set { plc3GroupString = value; }
        }

        private int plc3UpdateRate;
        [CategoryAttribute("3�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("С��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC3UpdateRate
        {
            get { return plc3UpdateRate; }
            set { plc3UpdateRate = value; }
        }

        private string plc4ServerName;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC4ServerName
        {
            get { return plc4ServerName; }
            set { plc4ServerName = value; }
        }

        private string plc4ServerIp;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC4ServerIP
        {
            get { return plc4ServerIp; }
            set { plc4ServerIp = value; }
        }


        private string plc4GroupString;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("4�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC4GroupString
        {
            get { return plc4GroupString; }
            set { plc4GroupString = value; }
        }

        private int plc4UpdateRate;
        [CategoryAttribute("4�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("4�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC4UpdateRate
        {
            get { return plc4UpdateRate; }
            set { plc4UpdateRate = value; }
        }

        private string plc5ServerName;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC5ServerName
        {
            get { return plc5ServerName; }
            set { plc5ServerName = value; }
        }

        private string plc5ServerIp;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC5ServerIP
        {
            get { return plc5ServerIp; }
            set { plc5ServerIp = value; }
        }


        private string plc5GroupString;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("5�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC5GroupString
        {
            get { return plc5GroupString; }
            set { plc5GroupString = value; }
        }

        private int plc5UpdateRate;
        [CategoryAttribute("5�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("5�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC5UpdateRate
        {
            get { return plc5UpdateRate; }
            set { plc5UpdateRate = value; }
        }
        ///
        private string plc6ServerName;
        [CategoryAttribute("6�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC6ServerName
        {
            get { return plc6ServerName; }
            set { plc6ServerName = value; }
        }

        private string plc6ServerIp;
        [CategoryAttribute("6�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC6ServerIP
        {
            get { return plc6ServerIp; }
            set { plc6ServerIp = value; }
        }


        private string plc6GroupString;
        [CategoryAttribute("6�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("6�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC6GroupString
        {
            get { return plc6GroupString; }
            set { plc6GroupString = value; }
        }

        private int plc6UpdateRate;
        [CategoryAttribute("6�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("6�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC6UpdateRate
        {
            get { return plc6UpdateRate; }
            set { plc6UpdateRate = value; }
        }
        private string plc7ServerName;
        [CategoryAttribute("7�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("��������"), Chinese("��������")]
        public string PLC7ServerName
        {
            get { return plc7ServerName; }
            set { plc7ServerName = value; }
        }

        private string plc7ServerIp;
        [CategoryAttribute("7�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("�����ַIP"), Chinese("����IP")]
        public string PLC7ServerIP
        {
            get { return plc7ServerIp; }
            set { plc7ServerIp = value; }
        }


        private string plc7GroupString;
        [CategoryAttribute("7�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("7�ŶѶ��PLCͨѶ��������"), Chinese("��������")]
        public string PLC7GroupString
        {
            get { return plc7GroupString; }
            set { plc7GroupString = value; }
        }

        private int plc7UpdateRate;
        [CategoryAttribute("7�ŶѶ��PLCͨ�Ų���"), DescriptionAttribute("7�ŶѶ��PLCˢ��Ƶ��"), Chinese("ˢ��Ƶ��")]
        public int PLC7UpdateRate
        {
            get { return plc7UpdateRate; }
            set { plc7UpdateRate = value; }
        }
    }
}
