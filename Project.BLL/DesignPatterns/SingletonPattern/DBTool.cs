using Project.DAL.ContextClasses;

namespace Project.BLL.DesignPatterns.SingletonPattern
{
    public class DBTool
    {

        DBTool() { }

        static MyContext _dbInstance;

        public static MyContext DBInstance
        {
            get
            {
                if (_dbInstance == null)
                {
                    _dbInstance = new MyContext();
                }

                return _dbInstance;
            }
        }

    }   
}
