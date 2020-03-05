using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tethering
{
    public class DataSource : ISource
    {
        private int ID = 0;
        private void StreamWrite(BinaryWriter stream, string str, int len)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            for (int k = 0; k < len; k++)
            {
                if (k < bytes.Length) stream.Write(bytes[k]);
                else stream.Write((byte)0);
            }

        }
        private void StreamWriteFile(BinaryWriter stream, Backup backup)
        {
            stream.Write(backup.id);
            stream.Write(backup.TTLValue);
            StreamWrite(stream, backup.Date, 19);
        }
        public int Save(int value)
        {
            RegistryKey myKey = Registry.LocalMachine;
            RegistryKey x = myKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", true);
            x.SetValue("DefaultTTL", value);
            return 0;
        }

        public Backup Backup()
        {
            using (var file = File.Open("Backup.bin", FileMode.OpenOrCreate))
            using (var stream = new BinaryReader(file))
            {
                long fileSize = new FileInfo("Backup.bin").Length;
                ID = (int)fileSize / 27;
            }
            Backup Back = new Backup(ID,Get(), DateTime.Now.ToString());
            using (var stream = new BinaryWriter(File.Open("Backup.bin", FileMode.OpenOrCreate)))
            {
                stream.Seek(0, SeekOrigin.End);
                StreamWriteFile(stream, Back);
            }
            return Back;
        }
        public Backup StreamReadFile(BinaryReader stream)
        {
            int Id = stream.ReadInt32();
            int value = stream.ReadInt32();
            int count = 0;
            byte[] bytes = new byte[19];
            for (int k = 0; k < 19; k++)
            {
                bytes[k] = stream.ReadByte();
                if (bytes[k] != '\0') count++;
            }
            string Date = Encoding.UTF8.GetString(bytes,0,count);
            Backup Back = new Backup(Id,value, Date);
            return Back;
        }
        public List<Backup> GetBackupList()
        {
            var result = new List<Backup>();
            using (var file = File.Open("Backup.bin", FileMode.OpenOrCreate))
            using (var stream = new BinaryReader(file))
            {
                file.Seek(0, SeekOrigin.Begin);
                while (file.Position < file.Length)
                {
                    Backup x = StreamReadFile(stream);
                    if (x != null) result.Add(x);
                }

            }
            return result;

        }

        public Backup RestoreBackup(int id)
        {
            Backup x = null;
            var list=GetBackupList();
            foreach(var item in list)
            {
                if(item.id==id)
                {
                    x = item;
                    Save(item.TTLValue);
                }
            }
            return x;
        }


        public int Get()
        {

            try
            {
                RegistryKey myKey = Registry.LocalMachine;
                RegistryKey x = myKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", true);
                int znac = (int)x.GetValue("DefaultTTL");
                return znac;
            }
            catch
            {
                return 0;
            }


        }
    }
}
