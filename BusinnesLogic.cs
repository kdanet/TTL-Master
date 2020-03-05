using System.Collections.Generic;

namespace Tethering
{
    public class BusinnesLogic : ISource
    {
        private ISource dataSource;

        public BusinnesLogic(ISource source)
        {
            dataSource = source;
        }

        public int Save(int value)
        {
            dataSource.Save(value);
            return 0;
        }

        public Backup Backup()
        {
            var x = dataSource.Backup();
            return x;
        }
        public List<Backup> GetBackupList()
        {
            var x = dataSource.GetBackupList();
            return x;
        }

        public Backup RestoreBackup(int id)
        {
            var x = dataSource.RestoreBackup(id);
            return x;
        }

        public int Get()
        {
            int x = dataSource.Get();
            if(x==0)
            {
                throw new System.Exception();
            }
            return x;
        }

    }
}
