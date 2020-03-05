namespace Tethering
{
    public class Backup
    {
        public int id;
        public int TTLValue;
        public string Date;
        public Backup(int _id,int _t, string _d)
        {
            id = _id;
            TTLValue = _t;
            Date = _d;
        }
    }
}
