﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace SPIROC_DAQ
{
    
    interface Iversion
    {
        void test();
        void set_property(int id, uint value);
        uint get_property(int id);
        int bit_transform(ref byte[] bit_block);
        void save_settings(int settings_id);
        void recall_settings(int settings_id);
        string getTag();
        Dictionary <string ,int > settings { get; set; }
        string settingName { get; set; }
        void testoutput();
    }

    [Serializable]
    class SC_model: Iversion 
    {
        // member variables
        private uint[] config_data;
        private static int properties_num = 175;
        public string settingName
        {
            get;
            set;
        }
        public const int bit_length = 929;
        public String bit_string;
        // hello
        private const string cache_loc = ".\\cache\\";

        // const variable describe length of each config properties.
        // notice that Discriminator Mask config (36 bits) is divided to two group whose length is 18 bits 
        // It can be check or get from "Spiroc2abcd_chip.xls" file.
        private static readonly ushort[] property_length = new ushort[175] {1,1,1,1,12,8,1,1,1,1,1,2,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
            9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,6,1,1,4,1,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,
            8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,1,3,1,1,3,1,1,1,1,1,1,1,1,1,10,10,1,6,1,1,1,1,1,1,1,
            1,18,18,1,1,6,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,
            1,6,1,6,1,1,1,1,1,1,1,1,1};
        public Dictionary<string, int> settings { get; set; } = new Dictionary<string, int> ();
        

        // Initial method
        public SC_model()
        {
            // default settings
            int i;
            config_data = new uint[175];
            settingName = "default";
            settings.Add("TRIG_EXT", 0);
            settings.Add("FLAG_TDC_EXT",1);// change frequently
            settings.Add("START_RAMP_ADC_EXT", 2);
            settings.Add("START_RAMP_TDC_EXT", 3);
            settings.Add("ADC_GRAY", 4);
            settings.Add("CHIPID", 5);
            settings.Add("PROBE_OTA", 6);
            settings.Add("ENABLE_ANALOGUE_OUTPUT", 7);
            settings.Add("DISABLE_ANALOGUE_OUTPUT_PP", 8);
            settings.Add("NC", 9);
            settings.Add("EN_OR36", 10);// change frequently
            settings.Add("ADC_RAMP_SLOPE", 11);
            settings.Add("ADC_RAMP_CURRENT_SOURCE", 12);
            settings.Add("ADC_RAMP_INTEGRATOR", 13);
            for (i=0; i < 36; i++)
            {
                string Key = "INDAC" + i.ToString();
                settings.Add(Key.ToString(), i + 14);
            }
            settings.Add("CAP_HG_PA_COMPENSATION",50);
            settings.Add("NC2",51);
            settings.Add("FS",52);
            settings.Add("NC3",53);
            settings.Add("CAP_LG_PA_COMPENSATION",54);
            settings.Add("ENABLE_PREAMP_PP",55);
            for (i = 0; i < 36; i++)
            {
                string Key = "PREAMP_GAIN" + i.ToString();
                settings.Add(Key.ToString(), i + 56);
            }
            settings.Add("ENABLE_LG_SS_FOLLOWER_PP",92);
            settings.Add("LG_SS_TIME_CONSTANT", 93);// change frequently
            settings.Add("ENABLE_LG_SS_PP", 94);
            settings.Add("ENABLE_HG_SS_FOLLOWER_PP", 95);
            settings.Add("HG_SS_TIME_CONSTANT", 96);// change frequently
            settings.Add("ENABLE_HG_SS_PP", 97);
            settings.Add("FS_FOLLOWER_PP", 98);
            settings.Add("FS_PP", 99);
            settings.Add("BACKUP_SCA", 100); // change frequently
            settings.Add("SCA_PP", 101);
            settings.Add("EN_BANDGAP", 102);
            settings.Add("BANDGAP_PP", 103);
            settings.Add("EN_DAC", 104);
            settings.Add("DAC_PP", 105);
            settings.Add("TRIG_DAC", 106); // very important
            settings.Add("GAIN_DAC", 107);  // very important
            settings.Add("DELAY_START_RAMP_TDC_PP", 108);
            settings.Add("DELAY_START_RAMP_TDC", 109);
            settings.Add("TDC_RAMP_SLOPE_GC", 110);
            settings.Add("TDC_RAMP_PP", 111);
            settings.Add("ADC_DISCRI_PP", 112);
            settings.Add("GAIN_SELECT_DISCRI_PP", 113);
            settings.Add("AUTO_GAIN", 114); // change frequently
            settings.Add("GAIN_SELECT", 115); // change frequently
            settings.Add("ADC_EXT_INPUT", 116);
            settings.Add("SWITCH_TDC_ON", 117);
            settings.Add("DISCRIMINATOR_MASK1", 118);
            settings.Add("DISCRIMINATOR_MASK2", 119);
            settings.Add("NC4", 120);
            settings.Add("DISCRI_DELAY_PP", 121);
            settings.Add("DELAY_TRIGGER", 122); // change frequently
            for (i = 0; i < 36; i++)
            {
                string Key = "DISCRI_4BIT_ADJUST" + i.ToString();
                settings.Add(Key.ToString(), i + 123);
            }
            settings.Add("ADJUST_4BIT_DAC", 159); // value is when select 'fine'
            settings.Add("DAC_4BIT_PP", 160);
            settings.Add("TRIG_DISCRI_PP", 161);
            settings.Add("DELAY_VALIDHOLD_PP", 162);
            settings.Add("DELAY_VALIDHOLD", 163);
            settings.Add("DELAY_RSTCOL_PP", 164);
            settings.Add("DELAY_RSTCOL", 165);
            settings.Add("CLOCK_LVDS_RECEIVE", 166);
            settings.Add("POD", 167);
            settings.Add("END_READOUT", 168);
            settings.Add("START_READOUT", 169);
            settings.Add("CHIPSAT", 170);
            settings.Add("TRANSMITON2", 171);
            settings.Add("TRANSMITON1", 172);
            settings.Add("DOUT2", 173);
            settings.Add("DOUT1", 174);

            
            this.set_property(settings["TRIG_EXT"], 0);
            this.set_property(settings["FLAG_TDC_EXT"], 0);
            this.set_property(settings["START_RAMP_ADC_EXT"], 0);
            this.set_property(settings["START_RAMP_TDC_EXT"], 0);
            this.set_property(settings["ADC_GRAY"], 1);
            this.set_property(settings["CHIPID"], 0x80);
            this.set_property(settings["PROBE_OTA"], 0);
            this.set_property(settings["ENABLE_ANALOGUE_OUTPUT"], 1);
            this.set_property(settings["DISABLE_ANALOGUE_OUTPUT_PP"], 1);
            this.set_property(settings["NC"], 0);
            this.set_property(settings["EN_OR36"], 1);
            this.set_property(settings["ADC_RAMP_SLOPE"], 0);
            this.set_property(settings["ADC_RAMP_CURRENT_SOURCE"], 0); // PP 
            this.set_property(settings["ADC_RAMP_INTEGRATOR"], 0);     // PP
            for ( i = 0; i < 36; i++)
            {
                string Key = "INDAC" + i.ToString();
                this.set_property(settings[Key.ToString()], 0x1ff);
            }
            this.set_property(settings["CAP_HG_PA_COMPENSATION"], 0x0f);
            this.set_property(settings["NC2"], 0);
            this.set_property(settings["FS"], 1);
            this.set_property(settings["NC3"], 0);
            this.set_property(settings["CAP_LG_PA_COMPENSATION"], 0x0e);
            this.set_property(settings["ENABLE_PREAMP_PP"], 0);
            for ( i = 0; i < 36; i++)
            {
                string Key = "PREAMP_GAIN" + i.ToString();
                this.set_property(settings[Key.ToString ()], 0xd8);
            }
            //disable channel 0
            //this.set_property(settings.PREAMP_GAIN[0], 0xec);
            this.set_property(settings["ENABLE_LG_SS_FOLLOWER_PP"], 0);
            this.set_property(settings["LG_SS_TIME_CONSTANT"], 0x04);
            this.set_property(settings["ENABLE_LG_SS_PP"], 0);
            this.set_property(settings["ENABLE_HG_SS_FOLLOWER_PP"], 0);
            this.set_property(settings["HG_SS_TIME_CONSTANT"], 0x04);
            this.set_property(settings["ENABLE_HG_SS_PP"], 0);
            this.set_property(settings["FS_FOLLOWER_PP"], 0);
            this.set_property(settings["FS_PP"], 0);
            this.set_property(settings["BACKUP_SCA"], 0);
            this.set_property(settings["SCA_PP"], 0);
            this.set_property(settings["EN_BANDGAP"], 1);
            this.set_property(settings["BANDGAP_PP"], 1);
            this.set_property(settings["EN_DAC"], 1);
            this.set_property(settings["DAC_PP"], 1);
            this.set_property(settings["TRIG_DAC"], 0x0fa);
            this.set_property(settings["GAIN_DAC"], 0x1f4);
            this.set_property(settings["DELAY_START_RAMP_TDC_PP"], 0);
            this.set_property(settings["DELAY_START_RAMP_TDC"], 1);
            this.set_property(settings["TDC_RAMP_SLOPE_GC"], 0);
            this.set_property(settings["TDC_RAMP_PP"], 0);
            this.set_property(settings["ADC_DISCRI_PP"], 0);
            this.set_property(settings["GAIN_SELECT_DISCRI_PP"], 0);
            this.set_property(settings["AUTO_GAIN"], 0);
            this.set_property(settings["GAIN_SELECT"], 0);
            this.set_property(settings["ADC_EXT_INPUT"], 0);
            this.set_property(settings["SWITCH_TDC_ON"], 1);
            this.set_property(settings["DISCRIMINATOR_MASK1"], 0);
            this.set_property(settings["DISCRIMINATOR_MASK2"], 0);
            this.set_property(settings["NC4"], 0);
            this.set_property(settings["DISCRI_DELAY_PP"], 1);
            this.set_property(settings["DELAY_TRIGGER"], 0x02);

            for ( i = 0; i < 36; i++)
            {
                string Key = "DISCRI_4BIT_ADJUST" + i.ToString();
                this.set_property(settings[Key.ToString ()], 0);
            }

            this.set_property(settings["ADJUST_4BIT_DAC"], 0);
            this.set_property(settings["DAC_4BIT_PP"], 0);
            this.set_property(settings["TRIG_DISCRI_PP"], 0);
            this.set_property(settings["DELAY_VALIDHOLD_PP"], 0);
            this.set_property(settings["DELAY_VALIDHOLD"], 0x14);
            this.set_property(settings["DELAY_RSTCOL_PP"], 0);
            this.set_property(settings["DELAY_RSTCOL"], 0x14);
            this.set_property(settings["CLOCK_LVDS_RECEIVE"], 0);
            this.set_property(settings["POD"], 0);
            this.set_property(settings["END_READOUT"], 1);
            this.set_property(settings["START_READOUT"], 1);
            this.set_property(settings["CHIPSAT"], 1);
            this.set_property(settings["TRANSMITON2"], 1);
            this.set_property(settings["TRANSMITON1"], 1);
            this.set_property(settings["DOUT2"], 1);
            this.set_property(settings["DOUT1"], 1);

        }


        // test method redundant for any test function
        public void WriteMessage(string path, string msg)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("{0}\n", msg, DateTime.Now);
                    sw.Flush();
                }
            }
        }

        public  void testoutput()
        {
            foreach (KeyValuePair<string, int> kvp in settings)
            {

                string msg = kvp.Key + "     " + kvp.Value + "     " + get_property(kvp.Value) + "    " + property_length [kvp.Value];
                var path = @"c:\test.txt";
                WriteMessage(path, msg);
            }

        }

        public   void test()
        {
            int sum = 0;
            foreach (ushort len in property_length)
            {
                Console.WriteLine(sum);
                sum += len;
            }
            //Console.WriteLine(sum);
        }


        // need cooperate with PROPERTIES-ID table in settings.cs
        public  void set_property(int id, uint value)
        {
            uint max = uint.MaxValue;
            max = max >> (32 - property_length[id]);
            if (value > max )
            {
                value = value & max;
            }
            config_data[id] = value;   
        }

        public   uint get_property(int id)
        {
            return config_data[id];

        }

        public  int bit_transform(ref byte[] bit_block)
        {
            // to record how many bit has been transformed
            int bit_count = 0;
            int byte_count = 0; //byte_count shold be bit_count/8;

            StringBuilder bit_As_Char = new StringBuilder(1000);
            
            // now bit is as this
            // location 0   1   2   3   4   ... 13  14  15
            // bit      1   1   1   1   1   ... 1   x   x
            for(int i = 0; i < 175; i++)
            {
                // 将配置的bit串用字符串的形式保存
                bit_As_Char.Append(Convert.ToString(config_data[i], 2).PadLeft(property_length[i], '0'));
            }

            // reverse sequence of chars in bit_As_Char
            // now bit is as this
            // location 13  12  11  10  9   ... 3   2   1
            // bit      1   1   1   1   1   ... 1   1   1
            StringBuilder bitAsChar_MsbFirst = new StringBuilder(bit_length);
            for( int i = bit_length - 1; i>=0; i--)
            {
                bitAsChar_MsbFirst.Append(bit_As_Char[i]);
            }


            bit_string = bitAsChar_MsbFirst.ToString();


            // transform 'bit in char form' into real bit stream
            // MSB in byte is bigger conig bit
            while(bit_count + 8 < bit_length)
            {
                bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, 8),2);
                byte_count++;
                bit_count += 8;
            }

            
            bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, bit_length - bit_count).PadRight(8,'0'),2);

            // for example if congfig data is 1101 0100 10
            // so now the bit block is 0100 1010 1100
            // bit_block[0]: 0x4
            // bit_block[1]: 0xA
            // bit_block[2]: 0xC
            return byte_count+1;

        }

        public  void save_settings(int settings_id)
        {   
            // save SlowContorl
            // Serialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!Directory.Exists(cache_loc))
                Directory.CreateDirectory(cache_loc);

            FileStream fileStream = new FileStream(cache_path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, this);
            fileStream.Close();

        }
            
        public  void recall_settings(int settings_id)
        {
            // load SlowControl saving config
            // Deserialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!File.Exists(cache_path))
            {
                throw new InvalidOperationException("Settings doesn't exist");
            }


            FileStream fileStream = new FileStream(cache_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();

            // restore config_data and settingName property
            var tmp = b.Deserialize(fileStream) as SC_model;
            tmp.config_data.CopyTo(this.config_data, 0);
            this.settingName = tmp.settingName;

            fileStream.Close();

        }

        public string getTag()
        {
            string result;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\tTrigger_Ext:\t" + this.get_property(settings["TRIG_EXT"]).ToString());
            builder.AppendLine("\tPreamp 1:\t" + this.get_property(settings["PREAMP_GAIN0"]).ToString());
            builder.AppendLine("\tSwitch TDC On:\t" + this.get_property(settings["SWITCH_TDC_ON"]).ToString());
            builder.AppendLine("\tAuto Gain:\t" + this.get_property(settings["AUTO_GAIN"]).ToString());
            builder.AppendLine("\tGain Select:\t" + this.get_property(settings["GAIN_SELECT"]).ToString());
            builder.AppendLine("\tHigh Gain Shaper\t" + this.get_property(settings["HG_SS_TIME_CONSTANT"]).ToString());
            builder.AppendLine("\tTrigger Delay\t" + this.get_property(settings["DELAY_TRIGGER"]).ToString());
            builder.AppendLine("");
            result = builder.ToString();
            return result;
        }
    }
    [Serializable]
    class SC_model_2E : Iversion
    {
        private uint[] config_data;
        public string settingName
        {
            get;
            set;
        }
        public String bit_string;
        private const string cache_loc = ".\\cache\\";
        private static readonly ushort[] property_length = new ushort[191] {1,1,1,1,1,1,12,8,1,1,1,1,1,2,1,1,1,1,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,
            9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,1,1,1,1,1,1,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,10,10,1,1,1,1,1,1,1,1,1,18,18,1,1,8,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,1,1,4,1,6,1,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
        private static int properties_num = 191;
        public const int bit_length = 1186;
        public Dictionary<string, int> settings { get; set; } = new Dictionary<string, int>();
        // const variable describe length of each config properties.
        // notice that Discriminator Mask config (36 bits) is divided to two group whose length is 18 bits 
        // It can be check or get from "Spiroc2abcd_chip.xls" file.

        public SC_model_2E()
        {
            // default settings_2E
            config_data = new uint[191];
            settingName = "default";
            int i;
            settings.Add("Sel_Temp_sensor_to_ADC_GC", 0);
            settings.Add("NC", 1);
            settings.Add("TRIG_EXT", 2);// change frequently
            settings.Add("FLAG_TDC_EXT", 3);// change frequently
            settings.Add("START_RAMP_ADC_EXT", 4);
            settings.Add("START_RAMP_TDC_EXT", 5);
            settings.Add("ADC_GRAY", 6);
            settings.Add("CHIPID", 7);
            settings.Add("PROBE_OTA", 8);
            settings.Add("ENABLE_ANALOGUE_OUTPUT", 9);
            settings.Add("DISABLE_ANALOGUE_OUTPUT_PP", 10);
            settings.Add("NC2", 11);
            settings.Add("EN_OR36", 12);// change frequently
            settings.Add("ADC_RAMP_SLOPE", 13);
            settings.Add("ADC_RAMP_CURRENT_SOURCE", 14);
            settings.Add("ADC_RAMP_INTEGRATOR", 15);
            settings.Add("EN_input_dac", 16);
            settings.Add("GC_8_bit_DAC_reference", 17);
            for (i = 0; i < 36; i++)
            {
                string Key = "INDAC" + i.ToString();
                settings.Add(Key.ToString(), i + 18);
            }
            settings.Add("LG_PA_bias", 54);
            settings.Add("High_Gain_PreAmplifier", 55);//?
            settings.Add("EN_High_Gain_PA", 56);//?
            settings.Add("Low_Gain_PreAmplifier", 57);//?
            settings.Add("EN_Low_Gain_PA", 58);//?
            settings.Add("Fast_Shaper_on_LG", 59);//?
            for (i = 0; i < 36; i++)
            {
                string Key = "PREAMP_GAIN" + i.ToString();
                settings.Add(Key.ToString(), i + 60);
            }
            settings.Add("ENABLE_LG_SS_PP", 96);//?

            settings.Add("EN_Low_Gain_Slow_Shaper", 97);//?
            settings.Add("LG_SS_TIME_CONSTANT", 98);// change frequently
            settings.Add("ENABLE_HG_SS_PP", 99);//?
            settings.Add("ENABLE_HG_SS", 100);//?
            settings.Add("HG_SS_TIME_CONSTANT", 101);// change frequently

            settings.Add("FS_FOLLOWER_PP", 102);
            settings.Add("EN_FS", 103);
            settings.Add("FS_PP", 104);
            settings.Add("BACKUP_SCA", 105); // change frequently
            settings.Add("SCA_PP", 106);
            settings.Add("GC_Temp_sensor_high_current", 107);
            settings.Add("PP_Temp", 108);
            settings.Add("EN_Temp", 109);
            settings.Add("BANDGAP_PP", 110);
            settings.Add("EN_BANDGAP", 111);
            settings.Add("EN_DAC1", 112);
            settings.Add("DAC1_PP", 113);
            settings.Add("EN_DAC2", 114);
            settings.Add("PP_DAC2", 115);
            settings.Add("TRIG_DAC", 116); // very important
            settings.Add("GAIN_DAC", 117);  // very important
            settings.Add("TDC_RAMP_SLOPE_GC", 118);
            settings.Add("TDC_RAMP_EN", 119);
            settings.Add("TDC_RAMP_PP", 120);
            settings.Add("ADC_DISCRI_PP", 121);
            settings.Add("GAIN_SELECT_DISCRI_PP", 122);
            settings.Add("AUTO_GAIN", 123); // change frequently
            settings.Add("GAIN_SELECT", 124); // change frequently
            settings.Add("ADC_EXT_INPUT", 125);
            settings.Add("SWITCH_TDC_ON", 126);
            settings.Add("DISCRIMINATOR_MASK1", 127);
            settings.Add("DISCRIMINATOR_MASK2", 128);
            settings.Add("Discri_Delay_Vref_I_source_EN", 129);
            settings.Add("Discri_Delay_Vref_I_source_PP", 130);
            settings.Add("DELAY_TRIGGER", 131);
            for (i = 0; i < 36; i++)
            {
                string Key = "DISCRI_4BIT_ADJUST" + i.ToString();
                settings.Add(Key.ToString(), i + 132);
            }
            settings.Add("TRIG_DISCRI_PP", 168);
            settings.Add("DAC_4BIT_PP", 169);
            settings.Add("DISCRI_DELAY_PP", 170);
            settings.Add("NC3", 171);
            settings.Add("DELAY_VALIDHOLD_PP", 172);
            settings.Add("DELAY_VALIDHOLD", 173);
            settings.Add("DELAY_RSTCOL_PP", 174);
            settings.Add("DELAY_RSTCOL", 175);
            settings.Add("EN_LVDS_receiver_NoTrig", 176);
            settings.Add("PP_LVDS_receiver_NoTrig", 177);
            settings.Add("EN_LVDS_receiver_ValEvt", 178);
            settings.Add("PP_LVDS_receiver_ValEvt", 179);
            settings.Add("EN_LVDS_receiver_TrigExt", 180);
            settings.Add("PP_LVDS_receiver_TrigExt", 181);
            settings.Add("CLOCK_LVDS_RECEIVE", 182);
            settings.Add("POD", 183);
            settings.Add("END_READOUT", 184);
            settings.Add("START_READOUT", 185);
            settings.Add("CHIPSAT", 186);
            settings.Add("TRANSMITON2", 187);
            settings.Add("TRANSMITON1", 188);
            settings.Add("DOUT2", 189);
            settings.Add("DOUT1", 190);


            this.set_property(settings["Sel_Temp_sensor_to_ADC_GC"], 0);
            this.set_property(settings["TRIG_EXT"], 0);
            this.set_property(settings["FLAG_TDC_EXT"], 0);
            this.set_property(settings["START_RAMP_ADC_EXT"], 0);
            this.set_property(settings["START_RAMP_TDC_EXT"], 0);
            this.set_property(settings["ADC_GRAY"], 1);
            this.set_property(settings["CHIPID"], 0x80);
            this.set_property(settings["PROBE_OTA"], 0);
            this.set_property(settings["ENABLE_ANALOGUE_OUTPUT"], 1);
            this.set_property(settings["DISABLE_ANALOGUE_OUTPUT_PP"], 1);
            this.set_property(settings["NC"], 0);
            this.set_property(settings["EN_OR36"], 1);
            this.set_property(settings["ADC_RAMP_SLOPE"], 0);
            this.set_property(settings["ADC_RAMP_CURRENT_SOURCE"], 0); // PP 
            this.set_property(settings["ADC_RAMP_INTEGRATOR"], 0);     // PP
            this.set_property(settings["EN_input_dac"], 1);//?
            this.set_property(settings["GC_8_bit_DAC_reference"], 0);//?
            for (i = 0; i < 36; i++)
            {
                string Key = "INDAC" + i.ToString();
                this.set_property(settings[Key.ToString()], 0x1ff);
            }
            this.set_property(settings["LG_PA_bias"], 0);//?
            this.set_property(settings["High_Gain_PreAmplifier"], 0);//?
            this.set_property(settings["Low_Gain_PreAmplifier"], 0);//?
            this.set_property(settings["EN_High_Gain_PA"], 0);//?
            this.set_property(settings["EN_Low_Gain_PA"], 0);//?
            this.set_property(settings["Fast_Shaper_on_LG"], 0);//?
            this.set_property(settings["NC2"], 0);
            this.set_property(settings["NC3"], 0);

            for (i = 0; i < 36; i++)
            {
                string Key = "PREAMP_GAIN" + i.ToString();
                this.set_property(settings[Key.ToString()], 0xd8);
            }
            //disable channel 0
            //this.set_property(settings["PREAMP_GAIN[0], 0xec);
            this.set_property(settings["EN_Low_Gain_Slow_Shaper"], 0);//?
            this.set_property(settings["ENABLE_HG_SS"], 0);//?
            this.set_property(settings["EN_FS"], 0);//?
            this.set_property(settings["GC_Temp_sensor_high_current"], 0);//?
            this.set_property(settings["PP_Temp"], 0);//?
            this.set_property(settings["EN_Temp"], 0);//?
            this.set_property(settings["EN_DAC1"], 0);//?
            this.set_property(settings["DAC1_PP"], 0);//?
            this.set_property(settings["EN_DAC2"], 0);//?
            this.set_property(settings["PP_DAC2"], 0);//?
            this.set_property(settings["TDC_RAMP_EN"], 0);//?
            this.set_property(settings["Discri_Delay_Vref_I_source_EN"], 0);//?
            this.set_property(settings["Discri_Delay_Vref_I_source_PP"], 0);//?
            this.set_property(settings["EN_LVDS_receiver_NoTrig"], 0);//?
            this.set_property(settings["PP_LVDS_receiver_NoTrig"], 0);//?
            this.set_property(settings["EN_LVDS_receiver_TrigExt"], 0);//?
            this.set_property(settings["PP_LVDS_receiver_TrigExt"], 0);//?
            this.set_property(settings["EN_LVDS_receiver_ValEvt"], 0);//?
            this.set_property(settings["PP_LVDS_receiver_ValEvt"], 0); //?
            this.set_property(settings["LG_SS_TIME_CONSTANT"], 0x04);
            this.set_property(settings["ENABLE_LG_SS_PP"], 0);

            this.set_property(settings["HG_SS_TIME_CONSTANT"], 0x04);
            this.set_property(settings["ENABLE_HG_SS_PP"], 0);
            this.set_property(settings["FS_FOLLOWER_PP"], 0);
            this.set_property(settings["FS_PP"], 0);
            this.set_property(settings["BACKUP_SCA"], 0);
            this.set_property(settings["SCA_PP"], 0);
            this.set_property(settings["EN_BANDGAP"], 1);
            this.set_property(settings["BANDGAP_PP"], 1);
            this.set_property(settings["TRIG_DAC"], 0x0fa);
            this.set_property(settings["GAIN_DAC"], 0x1f4);

            this.set_property(settings["TDC_RAMP_SLOPE_GC"], 0);
            this.set_property(settings["TDC_RAMP_PP"], 0);
            this.set_property(settings["ADC_DISCRI_PP"], 0);
            this.set_property(settings["GAIN_SELECT_DISCRI_PP"], 0);
            this.set_property(settings["AUTO_GAIN"], 0);
            this.set_property(settings["GAIN_SELECT"], 0);
            this.set_property(settings["ADC_EXT_INPUT"], 0);
            this.set_property(settings["SWITCH_TDC_ON"], 1);
            this.set_property(settings["DISCRIMINATOR_MASK1"], 0);
            this.set_property(settings["DISCRIMINATOR_MASK2"], 0);
            this.set_property(settings["NC3"], 0);
            this.set_property(settings["DISCRI_DELAY_PP"], 1);
            this.set_property(settings["DELAY_TRIGGER"], 0x02);

            for (i = 0; i < 36; i++)
            {
                string Key = "DISCRI_4BIT_ADJUST" + i.ToString();
                this.set_property(settings[Key.ToString()], 0);
            }

            this.set_property(settings["DAC_4BIT_PP"], 0);
            this.set_property(settings["TRIG_DISCRI_PP"], 0);
            this.set_property(settings["DELAY_VALIDHOLD_PP"], 0);
            this.set_property(settings["DELAY_VALIDHOLD"], 0x14);
            this.set_property(settings["DELAY_RSTCOL_PP"], 0);
            this.set_property(settings["DELAY_RSTCOL"], 0x14);
            this.set_property(settings["CLOCK_LVDS_RECEIVE"], 0);
            this.set_property(settings["POD"], 0);
            this.set_property(settings["END_READOUT"], 1);
            this.set_property(settings["START_READOUT"], 1);
            this.set_property(settings["CHIPSAT"], 1);
            this.set_property(settings["TRANSMITON2"], 1);
            this.set_property(settings["TRANSMITON1"], 1);
            this.set_property(settings["DOUT2"], 1);
            this.set_property(settings["DOUT1"], 1);


        }
        public void WriteMessage(string path, string msg)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("{0}\n", msg, DateTime.Now);
                    sw.Flush();
                }
            }
        }
        public void testoutput()
        {
            foreach (KeyValuePair<string, int> kvp in settings)
            {

                string msg = kvp.Key + "     " + kvp.Value + "     " + get_property(kvp.Value) + "    " + property_length[kvp.Value];
                var path = @"c:\test.txt";
                WriteMessage(path, msg);
            }

        }

        public void test()
        {
            int sum = 0;
            foreach (ushort len in property_length)
            {
                Console.WriteLine(sum);
                sum += len;
            }
            //Console.WriteLine(sum);
        }

        public void set_property(int id, uint value)
        {
            uint max = uint.MaxValue;
            max = max >> (32 - property_length[id]);
            if (value > max)
            {
                value = value & max;
            }
            config_data[id] = value;
        }

        public uint get_property(int id)
        {
            return config_data[id];

        }

        public int bit_transform(ref byte[] bit_block)
        {
            // to record how many bit has been transformed
            int bit_count = 0;
            int byte_count = 0; //byte_count shold be bit_count/8;

            StringBuilder bit_As_Char = new StringBuilder(1000);

            // now bit is as this
            // location 0   1   2   3   4   ... 13  14  15
            // bit      1   1   1   1   1   ... 1   x   x
            for (int i = 0; i < 175; i++)
            {
                // 将配置的bit串用字符串的形式保存
                bit_As_Char.Append(Convert.ToString(config_data[i], 2).PadLeft(property_length[i], '0'));
            }

            // reverse sequence of chars in bit_As_Char
            // now bit is as this
            // location 13  12  11  10  9   ... 3   2   1
            // bit      1   1   1   1   1   ... 1   1   1
            StringBuilder bitAsChar_MsbFirst = new StringBuilder(bit_length);
            for (int i = bit_length - 1; i >= 0; i--)
            {
                bitAsChar_MsbFirst.Append(bit_As_Char[i]);
            }


            bit_string = bitAsChar_MsbFirst.ToString();


            // transform 'bit in char form' into real bit stream
            // MSB in byte is bigger conig bit
            while (bit_count + 8 < bit_length)
            {
                bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, 8), 2);
                byte_count++;
                bit_count += 8;
            }


            bit_block[byte_count] = Convert.ToByte(bit_string.Substring(bit_count, bit_length - bit_count).PadRight(8, '0'), 2);

            // for example if congfig data is 1101 0100 10
            // so now the bit block is 0100 1010 1100
            // bit_block[0]: 0x4
            // bit_block[1]: 0xA
            // bit_block[2]: 0xC
            return byte_count + 1;

        }

        public void save_settings(int settings_id)
        {
            // save SlowContorl
            // Serialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!Directory.Exists(cache_loc))
                Directory.CreateDirectory(cache_loc);

            FileStream fileStream = new FileStream(cache_path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, this);
            fileStream.Close();

        }

        public void recall_settings(int settings_id)
        {
            // load SlowControl saving config
            // Deserialize
            String cache_path = cache_loc + settings_id.ToString() + ".cache";

            if (!File.Exists(cache_path))
            {
                throw new InvalidOperationException("Settings doesn't exist");
            }


            FileStream fileStream = new FileStream(cache_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();

            // restore config_data and settingName property
            var tmp = b.Deserialize(fileStream) as SC_model_2E;
            tmp.config_data.CopyTo(this.config_data, 0);
            this.settingName = tmp.settingName;

            fileStream.Close();

        }

        public string getTag()
        {
            string result;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\tTrigger_Ext:\t" + this.get_property(settings["TRIG_EXT"]).ToString());
            builder.AppendLine("\tPreamp 1:\t" + this.get_property(settings["PREAMP_GAIN0"]).ToString());
            builder.AppendLine("\tSwitch TDC On:\t" + this.get_property(settings["SWITCH_TDC_ON"]).ToString());
            builder.AppendLine("\tAuto Gain:\t" + this.get_property(settings["AUTO_GAIN"]).ToString());
            builder.AppendLine("\tGain Select:\t" + this.get_property(settings["GAIN_SELECT"]).ToString());
            builder.AppendLine("\tHigh Gain Shaper\t" + this.get_property(settings["HG_SS_TIME_CONSTANT"]).ToString());
            builder.AppendLine("\tTrigger Delay\t" + this.get_property(settings["DELAY_TRIGGER"]).ToString());
            builder.AppendLine("");
            result = builder.ToString();
            return result;
        }
    }
}

