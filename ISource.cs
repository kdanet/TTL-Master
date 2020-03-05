using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tethering
{
    public interface ISource
    {
        int Save(int id);

        List<Backup> GetBackupList();

        Backup RestoreBackup(int id);

        Backup Backup();

        int Get();

    }
}
